using System;
using System.Collections;
using System.Collections.Generic;
using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using Orders;
using Progression;
using UnityEngine;
using Zenject;

public class SmeltingActions : IAction, IInitialisation, IFixedExecute, IExecute, ICleanUp
{
    private const float MINUTE = 60f;
    private const int SMELTING_PER_MINUTE_MIN = 45;
    private const int SMELTING_PER_MINUTE_MAX = 60;
    private const float MAX_TEMPERATURE = 100f;
    private const float TARGET_TEMPERATURE = 85f;
    private const float TEMPERATURE_TIME = 5f;
    private const float TEMPERATURE_ADD_STEP = 10f;
    private const float TEMPERATURE_REMOVE_STEP = 15f;
    private const float TEMPERATURE_REMOVE_COOLING_STEP = 8f;
    private const float COOLING_TIME = 1.6f;
    private const float SIGNAL_SPEED = 10f;
    private const float BELLOWS_ANIMATION_TIME = 0.5f;
    
    private SmeltingView _smeltingView;
    private GameEventBus _gameEventBus;
    private StateEventsBus _stateEvents;
    private OrdersEventBus _ordersEventBus;
    private ProgressionData _progressionData;
    private AudioEventBus _audioEventBus;
    private TutorialEventBus _tutorialEventBus;

    private UpgradesMetaData _upgradesMetaData;
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
    private System.Random _random;
    private Timer _bellowsAnimationTimer;

    [Inject]
    public void Construct(SmeltingView smeltingView, GameEventBus gameEventBus, StateEventsBus stateEvents,
        OrdersEventBus ordersEventBus, ProgressionData progressionData, AudioEventBus audioEventBus,
        TutorialEventBus tutorialEventBus)
    {
        _smeltingView = smeltingView;
        _gameEventBus = gameEventBus;
        _stateEvents = stateEvents;
        _ordersEventBus = ordersEventBus;
        _progressionData = progressionData;
        _audioEventBus = audioEventBus;
        _tutorialEventBus = tutorialEventBus;
    }
    
    public void Initialisation()
    {
        _stateEvents.OnSmeltingStateActivate += StartSmelting;
        _ordersEventBus.OnOrderEnded += StopProcess;
        _smeltingView.BellowsButton.Button.onClick.AddListener(BellowsAction);
        _isSmelting = false;
        _random = new System.Random();
        _upgradesMetaData = _progressionData.UpgradesMeta;
    }

    public void Cleanup()
    {
        _stateEvents.OnSmeltingStateActivate -= StartSmelting;
        _ordersEventBus.OnOrderEnded -= StopProcess;
        _smeltingView.BellowsButton.Button.onClick.RemoveListener(BellowsAction);
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
        if (_bellowsAnimationTimer != null)
        {
            if (_bellowsAnimationTimer.Wait())
            {
                _bellowsAnimationTimer = null;
                _smeltingView.BellowsButton.ButtonImage.sprite = _smeltingView.BellowsSprite;
            }
        }
    }
    
    private void BellowsAction()
    {
        _tutorialEventBus.OnSmeltingNearTriggerHit?.Invoke();
        _audioEventBus.OnPlaySound?.Invoke(AudioResourceID.Sound_Bellows);
        if (_triggeredSignalRect != null)
        {
            _temperature += TEMPERATURE_ADD_STEP;
            ClearTriggeredObject(_triggeredSignalRect.gameObject, true);
            _tutorialEventBus.OnSmeltingGoodHit?.Invoke();
            _coolingTimer = new Timer(COOLING_TIME);
        }
        else
        {
            _temperature -= TEMPERATURE_REMOVE_STEP;
        }
        _temperature = Mathf.Clamp(_temperature, 0, MAX_TEMPERATURE);
        _bellowsAnimationTimer = new Timer(BELLOWS_ANIMATION_TIME);
        _smeltingView.BellowsButton.ButtonImage.sprite = _smeltingView.ActiveBellowsSprite;
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
                _tutorialEventBus.OnSmeltingNearTrigger?.Invoke();
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
                SetupSignalTimer();
                GameObjectSpawnCallback callback = new GameObjectSpawnCallback();
                _gameEventBus.OnSpawnObject?.Invoke(PrefabID.UISmeltingSingal,
                    Vector3.zero, _smeltingView.SpawnZoneTransform, callback);
                AddBellowsSignal(callback.SpawnedObject);
            }
        }
    }

    private void AddBellowsSignal(GameObject signalObject)
    {
        RectTransform signalRect = signalObject.GetComponent<RectTransform>();
        signalRect.anchoredPosition = Vector2.zero;
        _activeSignalRects.Add(signalRect);
        signalRect.transform.localScale = Vector3.one;
    }

    private void StartSmelting()
    {
        _audioEventBus.OnPlayAmbientSound?.Invoke(AudioResourceID.Ambient_Fire);
        _isSmelting = true;
        _activeSignalRects = new List<RectTransform>();
        _triggeredSignalRect = null;
        SetupSignalTimer();
        _coolingTimer = new Timer(COOLING_TIME);
        _temperatureTimer = null;
        _smeltingView.TemperatureSlider.maxValue = MAX_TEMPERATURE;
        _temperature = 0f;
        _tutorialEventBus.OnSmeltingStarted?.Invoke();
    }

    private void SetupSignalTimer()
    {
        int minimalTime = SMELTING_PER_MINUTE_MIN;
        int maximalTime = SMELTING_PER_MINUTE_MAX;
        if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.SmeltingHeat))
        {
            float upgradeMultipler = _upgradesMetaData.Upgrades[UpgradeName.SmeltingHeat].GetUpgradeData();
            minimalTime += 2 * (int)upgradeMultipler;
            maximalTime += 12 * (int)upgradeMultipler;
        }
        int randomSmeltingPerMinute = _random.Next(minimalTime, maximalTime);
        float cooldown = MINUTE / randomSmeltingPerMinute;
        _smeltingTimer = new Timer(cooldown);
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
            _smeltingView.TimerText.gameObject.SetActive(true);
             _temperatureTimer.GetRemainingTime().ToString();
            float secondsRemaining = _temperatureTimer.GetRoundedRemainingTime(2);
            TimeSpan time = TimeSpan.FromSeconds(secondsRemaining);
            _smeltingView.TimerText.text = time.ToString("mm':'ss");
            if (_temperatureTimer.Wait())
            {
                EndProcess();
            }
        }
        else
        {
            _smeltingView.TimerText.gameObject.SetActive(false);
        }
    }
    
    private void StopProcess(ActiveOrder order = null)
    {
        foreach (RectTransform signalTransform in _activeSignalRects)
        {
            GameObject.Destroy(signalTransform.gameObject);
        }
        _activeSignalRects = new List<RectTransform>();
        _triggeredSignalRect = null;
        _isSmelting = false;
        _temperatureTimer = null;
        _isTriggeredObjectSet = false;
    }

    private void EndProcess()
    {
        StopProcess();
        _audioEventBus.OnStopAmbientSound?.Invoke();
        _stateEvents.OnForgingStateActivate?.Invoke(0.5f);
    }
}