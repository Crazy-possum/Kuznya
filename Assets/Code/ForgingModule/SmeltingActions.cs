using System.Collections;
using System.Collections.Generic;
using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using Orders;
using UnityEngine;
using Zenject;

public class SmeltingActions : IAction, IInitialisation, IFixedExecute, IExecute, ICleanUp
{
    //TODO: Configure timers, add random
    private const float MINUTE = 60f;
    private const float SMELTING_BPM = 60f;
    private const float MAX_TEMPERATURE = 100f;
    private const float TARGET_TEMPERATURE = 85f;
    private const float TEMPERATURE_TIME = 5f;
    private const float TEMPERATURE_ADD_STEP = 5f;
    private const float TEMPERATURE_REMOVE_STEP = 8f;
    private const float TEMPERATURE_REMOVE_COOLING_STEP = 2f;
    private const float COOLING_TIME = 0.5f;
    private const float SIGNAL_SPEED = 5f;
    
    private SmeltingView _smeltingView;
    private GameEventBus _gameEventBus;
    private StateEventsBus _stateEvents;
    private OrdersEventBus _ordersEventBus;
    
    private Timer _smeltingTimer;
    private bool _isSmelting;
    private Timer _temperatureTimer;
    private float _temperature;
    private List<RectTransform> _activeSignalRects;
    private Timer _coolingTimer;
    private bool _isCooling;
    private SignalObjectView _triggeredSignalRect;
    private SignalObjectView _lastTriggeredSignalRect;
    private bool _isTriggeredObjectSet;

    [Inject]
    public void Construct(SmeltingView smeltingView, GameEventBus gameEventBus, StateEventsBus stateEvents,
        OrdersEventBus ordersEventBus)
    {
        _smeltingView = smeltingView;
        _gameEventBus = gameEventBus;
        _stateEvents = stateEvents;
        _ordersEventBus = ordersEventBus;
    }
    
    public void Initialisation()
    {
        _stateEvents.OnSmeltingStateActivate += StartSmelting;
        _ordersEventBus.OnOrderEnded += StopProcess;
        _smeltingView.BellowsButton.onClick.AddListener(BellowsAction);
        _isSmelting = false;
    }

    public void Cleanup()
    {
        _stateEvents.OnSmeltingStateActivate -= StartSmelting;
        _ordersEventBus.OnOrderEnded -= StopProcess;
        _smeltingView.BellowsButton.onClick.RemoveListener(BellowsAction);
    }
    
    public void FixedExecute(float fixedDeltaTime)
    {
        if (_isSmelting)
        {
            CheckTemperature();
            TrySpawnNewBellowsSignal();
            CheckEndCondition();
            ActCooling();
            UpdateUI();
        }
    }
    
    public void Execute(float deltaTime)
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isSmelting)
        {
            BellowsAction();
        }
    }

    private void ActCooling()
    {
        if (_coolingTimer != null)
        {
            if (_coolingTimer.Wait())
            {
                if (_isCooling)
                {
                    _temperature -= TEMPERATURE_REMOVE_COOLING_STEP;
                    _temperature = Mathf.Clamp(_temperature, 0, MAX_TEMPERATURE);
                }
                else if(!_isCooling)
                {
                    _isCooling = true;
                }
            }
        }
    }

    private void UpdateUI()
    {
        MoveActiveSignals();
        _smeltingView.TemperatureSlider.value = _temperature;
    }
    
    private void BellowsAction()
    {
        if (_triggeredSignalRect != null)
        {
            _temperature += TEMPERATURE_ADD_STEP;
            ClearTriggeredObject(_triggeredSignalRect.gameObject, true);
        }
        else
        {
            _temperature -= TEMPERATURE_REMOVE_STEP;
        }
        _temperature = Mathf.Clamp(_temperature, 0, MAX_TEMPERATURE);
        _isCooling = false;
    }
    
    private void SetTriggeredObject(GameObject signalObject)
    {
        SignalObjectView signalView;
        if (signalObject.TryGetComponent<SignalObjectView>(out signalView))
        {
            if (_triggeredSignalRect == null)
            {
                signalView.SignalImage.sprite = signalView.SignalActiveSprite;
                _lastTriggeredSignalRect = null;
                _triggeredSignalRect = signalView;
                _isTriggeredObjectSet = true;
                //Debug.Log($"{_triggeredSignalRect.gameObject.GetInstanceID()} set");
            }

        }
    }
    
    private void ClearTriggeredObject(GameObject signalObject, bool isGood)
    {
        SignalObjectView signalView;
        if (signalObject.TryGetComponent<SignalObjectView>(out signalView))
        {
            if (_triggeredSignalRect != null)
            {
                if (isGood)
                {
                    signalView.SignalImage.sprite = signalView.SignalGoodSprite;
                }
                else
                {
                    signalView.SignalImage.sprite = signalView.SignalBadSprite;
                }

                _lastTriggeredSignalRect = _triggeredSignalRect;
                //Debug.Log($"{_triggeredSignalRect.gameObject.GetInstanceID()} cleared");
                _triggeredSignalRect = null;
                _isTriggeredObjectSet = false;
            }
        }
    }

    private void MoveActiveSignals()
    {
        if (_activeSignalRects.Count > 0)
        {
            List<RectTransform> signalsToRemove = new List<RectTransform>();
            foreach (RectTransform signalRect in _activeSignalRects)
            {
                Vector3 targetPosition = _smeltingView.EndZoneTransform.anchoredPosition;
                float distance = Mathf.Abs(targetPosition.x - signalRect.anchoredPosition.x);
                float step = SIGNAL_SPEED;

                if (distance > step)
                {
                    Vector2 direction = new Vector2(-1, 0);
                    signalRect.anchoredPosition += direction * step;
                }
                else
                {
                    signalsToRemove.Add(signalRect);
                }

                CheckRectPosition(signalRect);
                
            }

            foreach (RectTransform signalRect in signalsToRemove)
            {
                _activeSignalRects.Remove(signalRect);
                GameObject.Destroy(signalRect.gameObject);
            }
        }
    }

    private void CheckRectPosition(RectTransform signalRect)
    {
        float positionX = signalRect.anchoredPosition.x;
        RectTransform targetPosition = _smeltingView.TargetZoneTransform;
        float targetDelta = targetPosition.rect.width / 2f;
        float targetPositionXRight = targetPosition.anchoredPosition.x + targetDelta;
        float targetPositionXLeft = targetPosition.anchoredPosition.x - targetDelta;
        bool wasTriggered = false;
        if (_lastTriggeredSignalRect != null)
        {
            wasTriggered = _lastTriggeredSignalRect.gameObject == signalRect.gameObject;
        }
            
        if (positionX < targetPositionXRight && positionX > targetPositionXLeft)
        {
            if (!_isTriggeredObjectSet && !wasTriggered)
            {
                SetTriggeredObject(signalRect.gameObject);
            }
        }
        else if (positionX < targetPositionXLeft && positionX > targetPositionXLeft - SIGNAL_SPEED * 2f)
        {
            if (_isTriggeredObjectSet)
            {
                ClearTriggeredObject(signalRect.gameObject, false);
            }
        }
    }

    private void TrySpawnNewBellowsSignal()
    {
        if (_smeltingTimer != null)
        {
            if (_smeltingTimer.Wait())
            {
                _gameEventBus.OnObjectSpawned += AddBellowsSignal;
                _gameEventBus.OnSpawnObject?.Invoke(PrefabID.UISmeltingSingal,
                    Vector3.zero, _smeltingView.SpawnZoneTransform);
            }
        }
    }

    private void AddBellowsSignal(GameObject signalObject)
    {
        RectTransform signalRect = signalObject.GetComponent<RectTransform>();
        signalRect.anchoredPosition = Vector2.zero;
        _activeSignalRects.Add(signalRect);
        _gameEventBus.OnObjectSpawned -= AddBellowsSignal;
    }

    private void StartSmelting()
    {
        _isSmelting = true;
        _activeSignalRects = new List<RectTransform>();
        _triggeredSignalRect = null;
        float cooldown = MINUTE / SMELTING_BPM;
        _smeltingTimer = new Timer(cooldown);
        _coolingTimer = new Timer(COOLING_TIME);
        _temperatureTimer = null;
        _smeltingView.TemperatureSlider.maxValue = MAX_TEMPERATURE;
        _temperature = 0f;
    }
    
    private void CheckTemperature()
    {
        if (_temperature > MAX_TEMPERATURE)
        {
            _temperature = MAX_TEMPERATURE;
        }
        if (_temperature >= TARGET_TEMPERATURE)
        {
            if (_temperatureTimer == null)
            {
                _temperatureTimer = new Timer(TEMPERATURE_TIME);
            }
        }
        else
        {
            _temperatureTimer = null;
        }
    }
    
    private void CheckEndCondition()
    {
        if (_temperatureTimer != null && _isSmelting)
        {
            if (_temperatureTimer.Wait())
            {
                EndProcess();
            }
        }
    }
    
    private void StopProcess(ActiveOrder order = null)
    {
        _activeSignalRects = new List<RectTransform>();
        _triggeredSignalRect = null;
        _isSmelting = false;
        _temperatureTimer = null;
    }

    private void EndProcess()
    {
        StopProcess();
        _stateEvents.OnForgingStateActivate?.Invoke();
        //TODO : Add points to next stage
    }
}