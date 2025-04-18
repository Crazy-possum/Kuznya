using System;
using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using Orders;
using UnityEngine;
using Zenject;

public class HardeningActions : IAction, IInitialisation, ICleanUp, IFixedExecute
{
    private const int ZONE_SIZE_DELTA = 20;
    private const int MAX_PROGRESS = 5;
    private const int COST_MULTIPLER = 10;
    
    private HardeningUIView _uiView;
    private ForgingEventBus _eventBus;
    private StateEventsBus _stateEventsBus;
    private OrdersEventBus _ordersEventBus;

    private int _currentProgress;
    private int _currentScore;
    private bool _isRunning;
    private int _basicAddingScore;
    private int _currentBasicCost;
    private bool _isMovingRight;
    private Vector2 _greenZoneInitialSize;
    private Vector2 _yellowZoneInitialSize;
    
    [Inject]
    public void Construct(HardeningUIView uiView, ForgingEventBus forgingEventBus,
        StateEventsBus stateEventsBus, OrdersEventBus ordersEventBus)
    {
        _uiView = uiView;
        _eventBus = forgingEventBus;
        _stateEventsBus = stateEventsBus;
        _ordersEventBus = ordersEventBus;
    }
    
    
    public void Initialisation()
    {
        _eventBus.OnForgingFinished += StartHardening;
        _ordersEventBus.OnOrderStarted += StartOrder;
        _ordersEventBus.OnOrderEnded += StopProcess;
        _uiView.HardeningButton.onClick.AddListener(ActHardening);
        _greenZoneInitialSize = _uiView.TargetZoneGreen.sizeDelta;
        _yellowZoneInitialSize = _uiView.TargetZoneYellow.sizeDelta;
    }
    
    public void FixedExecute(float fixedDeltaTime)
    {
        if (_isRunning)
        {
            MoveSlider();
            UpdateUI();
        }
    }

    public void Cleanup()
    {
        _eventBus.OnForgingFinished -= StartHardening;
        _ordersEventBus.OnOrderStarted -= StartOrder;
        _ordersEventBus.OnOrderEnded -= StopProcess;
        _uiView.HardeningButton.onClick.RemoveListener(ActHardening);
    }
    
    private void MoveSlider()
    {
        if (_isMovingRight)
        {
            _uiView.HardeningSlider.value += 1;
            if(_uiView.HardeningSlider.value >= _uiView.HardeningSlider.maxValue)
            {
                _isMovingRight = false;
            }
        }
        else
        {
            _uiView.HardeningSlider.value -= 1;
            if(_uiView.HardeningSlider.value <= _uiView.HardeningSlider.minValue)
            {
                _isMovingRight = true;
            }
        }
    }

    private void StartOrder(ActiveOrder order)
    {
        _currentBasicCost = order.BasicCost;
        _basicAddingScore = (_currentBasicCost * COST_MULTIPLER) / MAX_PROGRESS;
    }
    
    private void StopProcess(ActiveOrder order)
    {
        ClearProgress();
        _uiView.gameObject.SetActive(false);
    }
    
    private void ActHardening()
    {
        if (_isRunning)
        {
            _currentProgress++;
            bool isInGreenZone = CheckGreenZone();
            bool isInYellowZone = CheckYellowZone();
            if (isInGreenZone)
            {
                _currentScore += _basicAddingScore;
                //Debug.Log($"{_basicAddingScore} added to score");
            }
            else if (isInYellowZone)
            {
                _currentScore += _basicAddingScore / 2;
                //Debug.Log($"{_basicAddingScore / 2} added to score");
            }
            else
            {
                //Debug.Log($"Nothing added to score");
            }
            if (_currentProgress >= MAX_PROGRESS)
            {
                EndProcess();
                return;
            }
            ChangeZonesSize();
            UpdateUI();
        }
    }

    private void ChangeZonesSize()
    {
        _uiView.TargetZoneGreen.sizeDelta = new Vector2(_uiView.TargetZoneGreen.sizeDelta.x - ZONE_SIZE_DELTA,
            _uiView.TargetZoneGreen.sizeDelta.y);
        _uiView.TargetZoneYellow.sizeDelta = new Vector2(_uiView.TargetZoneYellow.sizeDelta.x - ZONE_SIZE_DELTA,
            _uiView.TargetZoneYellow.sizeDelta.y);
    }

    private bool CheckGreenZone()
    {
        return EngineExtention.IsObjectInsideUIRectHorizontal(_uiView.TargetZoneGreen,
            _uiView.SliderCheckZone);
    }
    
    private bool CheckYellowZone()
    {
        return EngineExtention.IsObjectInsideUIRectHorizontal(_uiView.TargetZoneYellow,
            _uiView.SliderCheckZone);
    }

    private void StartHardening(int score)
    {
        _currentScore = score;
        _currentProgress = 0;
        _isRunning = true;
    }
    
    private void EndProcess()
    {
        _eventBus.OnHardeningFinished?.Invoke(_currentScore);
        ClearProgress();
        _stateEventsBus.OnResultsStateActivate?.Invoke();
        _uiView.gameObject.SetActive(false);
    }
    
    private void ClearProgress()
    {
        _currentProgress = 0;
        _currentScore = 0;
        _basicAddingScore = 0;
        _isRunning = false;
        _uiView.TargetZoneGreen.sizeDelta = _greenZoneInitialSize;
        _uiView.TargetZoneYellow.sizeDelta = _yellowZoneInitialSize;
    }
    
    private void UpdateUI()
    {
        _uiView.ScoreText.text = _currentScore.ToString();
    }

}