using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using Orders;
using UnityEngine;
using Zenject;
using Random = System.Random;

public class HardeningActions : IAction, IInitialisation, ICleanUp, IFixedExecute
{
    private const int COST_MULTIPLER = 50;
    private const float HARDENING_TIME = 10f;
    private const float STATE1_ZONE_START = 0.16f;
    private const float STATE2_ZONE_START = 0.5f;
    private const float STATE3_ZONE_START = 0.84f;
    private const float ZONE_HALF_SIZE = 0.10f;
    
    private HardeningUIView _hardeningUIView;
    private ForgingEventBus _forgingEventBus;
    private StateEventsBus _stateEventsBus;
    private OrdersEventBus _ordersEventBus;
    private AudioEventBus _audioEventBus;

    private float _currentScore;
    private bool _isRunning;
    private bool _isInCorrectZone;
    private float _basicAddingScore;
    private int _currentBasicCost;
    private Timer _hardeningTimer;
    private Timer _stateTimer;
    private int _currentZoneIndex;
    private System.Random _random;
    
    [Inject]
    public void Initialize(HardeningUIView hardeningUIView, ForgingEventBus forgingEventBus,
        StateEventsBus stateEventsBus, OrdersEventBus ordersEventBus, AudioEventBus audioEventBus)
    {
        _hardeningUIView = hardeningUIView;
        _forgingEventBus = forgingEventBus;
        _stateEventsBus = stateEventsBus;
        _ordersEventBus = ordersEventBus;
        _audioEventBus = audioEventBus;
    }
    
    public void Initialisation()
    {
        _forgingEventBus.OnForgingFinished += StartHardening;
        _ordersEventBus.OnOrderStarted += StartOrder;
        _ordersEventBus.OnOrderEnded += StopProcess;
        _random = new System.Random();
    }

    public void Cleanup()
    {
        _forgingEventBus.OnForgingFinished -= StartHardening;
        _ordersEventBus.OnOrderStarted -= StartOrder;
        _ordersEventBus.OnOrderEnded -= StopProcess;
    }
    
    public void FixedExecute(float fixedDeltaTime)
    {
        if (_isRunning)
        {
            if (_hardeningTimer != null)
            {
                if (_hardeningTimer.Wait())
                {
                    EndProcess();
                }
                else
                {
                    if (_isInCorrectZone)
                    {
                        _currentScore += _basicAddingScore;
                        _hardeningUIView.UpdateScore((int)_currentScore);
                        if(!CheckCurrentCondition())
                        {
                            _audioEventBus.OnStopSound?.Invoke();
                            _isInCorrectZone = false;
                        }
                    }
                    else if(CheckCurrentCondition())
                    {
                        _audioEventBus.OnPlaySoundLoop?.Invoke(AudioResourceID.Sound_Hardening);
                        _isInCorrectZone = true;
                    }
                }
            }

            if (_stateTimer != null)
            {
                if (_stateTimer.Wait())
                {
                    _audioEventBus.OnStopSound?.Invoke();
                    ChangeZoneIndex();
                }
            }
        }
    }

    private bool CheckCurrentCondition()
    {
        float currentTopLimit = 1;
        float currentBottomLimit = 0;
        if (_currentZoneIndex == 0)
        {
            currentTopLimit = STATE1_ZONE_START - ZONE_HALF_SIZE;
            currentBottomLimit = STATE1_ZONE_START + ZONE_HALF_SIZE;
        }
        else if(_currentZoneIndex == 1)
        {
            currentTopLimit = STATE2_ZONE_START - ZONE_HALF_SIZE;
            currentBottomLimit = STATE2_ZONE_START + ZONE_HALF_SIZE;
        }
        else if(_currentZoneIndex == 2)
        {
            currentTopLimit = STATE3_ZONE_START - ZONE_HALF_SIZE;
            currentBottomLimit = STATE3_ZONE_START + ZONE_HALF_SIZE;
        }
        if (_hardeningUIView.HardeningSlider.value >= currentTopLimit &&
            _hardeningUIView.HardeningSlider.value <= currentBottomLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void StartOrder(ActiveOrder order)
    {
        _currentBasicCost = order.BasicCost;
        float timerTicks = HARDENING_TIME / Time.fixedDeltaTime;
        _basicAddingScore = (_currentBasicCost * COST_MULTIPLER) / timerTicks;
    }
        
    public void StartHardening(int score)
    {
        if (_isRunning) return;
        _hardeningTimer = new Timer(HARDENING_TIME);
        _stateTimer = new Timer(HARDENING_TIME / 5);
        _isRunning = true;
        _currentScore = score;
        ChangeZoneIndex();
        _hardeningUIView.UpdateScore((int)_currentScore);
    }

    private void ChangeZoneIndex()
    {
        int index = _random.Next(0, 3);
        int tryCount = 0;
        while (index == _currentZoneIndex && tryCount < 10)
        {
            tryCount++;
            index = _random.Next(0, 3);
        }
        _audioEventBus.OnPlaySound?.Invoke(AudioResourceID.Sound_Click_1);
        _currentZoneIndex = index;
    }
    
    private void StopProcess(ActiveOrder order)
    {
        ClearProgress();
        _hardeningUIView.gameObject.SetActive(false);
    }
    
    private void EndProcess()
    {
        _forgingEventBus.OnHardeningFinished?.Invoke((int)_currentScore);
        ClearProgress();
        _stateEventsBus.OnSharpeningStateActivate?.Invoke();
        _hardeningUIView.gameObject.SetActive(false);
    }
    
    private void ClearProgress()
    {
        _audioEventBus.OnStopSound?.Invoke();
        _currentScore = 0;
        _isRunning = false;
        _hardeningTimer = null;
        _stateTimer = null;
    }


}