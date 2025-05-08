using System;
using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using Orders;
using Progression;
using UnityEngine;
using Zenject;

public class SharpeningActions : IAction, IInitialisation, ICleanUp, IFixedExecute
{
    private const int ZONE_SIZE_DELTA = 20;
    private const int MAX_PROGRESS = 5;
    private const int COST_MULTIPLER = 50;
    
    private SharpeningUIView _uiView;
    private ForgingEventBus _eventBus;
    private StateEventsBus _stateEventsBus;
    private OrdersEventBus _ordersEventBus;
    private ProgressionData _progressionData;

    private UpgradesMetaData _upgradesMetaData;
    private int _currentProgress;
    private int _currentScore;
    private bool _isRunning;
    private int _basicAddingScore;
    private int _currentBasicCost;
    private bool _isMovingRight;
    private Vector2 _greenZoneInitialSize;
    private Vector2 _yellowZoneInitialSize;
    private int _currentMaxProgress;
    
    [Inject]
    public void Construct(SharpeningUIView uiView, ForgingEventBus forgingEventBus,
        StateEventsBus stateEventsBus, OrdersEventBus ordersEventBus, ProgressionData progressionData)
    {
        _uiView = uiView;
        _eventBus = forgingEventBus;
        _stateEventsBus = stateEventsBus;
        _ordersEventBus = ordersEventBus;
        _progressionData = progressionData;
    }
    
    
    public void Initialisation()
    {
        _eventBus.OnHardeningFinished += StartSharpening;
        _ordersEventBus.OnOrderStarted += StartOrder;
        _ordersEventBus.OnOrderEnded += StopProcess;
        _uiView.HardeningButton.onClick.AddListener(ActSharpening);
        _greenZoneInitialSize = _uiView.TargetZoneGreen.sizeDelta;
        _yellowZoneInitialSize = _uiView.TargetZoneYellow.sizeDelta;
        _upgradesMetaData = _progressionData.UpgradesMeta;
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
        _eventBus.OnForgingFinished -= StartSharpening;
        _ordersEventBus.OnOrderStarted -= StartOrder;
        _ordersEventBus.OnOrderEnded -= StopProcess;
        _uiView.HardeningButton.onClick.RemoveListener(ActSharpening);
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
        float additionalGoodScoreMultipler = 0;
        if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.ForgingScore))
        {
            additionalGoodScoreMultipler = _upgradesMetaData.Upgrades[UpgradeName.ForgingScore].GetUpgradeData();
        }

        _currentMaxProgress = MAX_PROGRESS;
        if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.SharpeningSpeed))
        {
            _currentMaxProgress -= (int)_upgradesMetaData.Upgrades[UpgradeName.SharpeningSpeed].GetUpgradeData();
        }

        if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.SharpeningZoneValue))
        {
            ChangeStartZonesSize(_upgradesMetaData.Upgrades[UpgradeName.SharpeningZoneValue].GetUpgradeData());
        }
        
        _basicAddingScore = (int)((_currentBasicCost * COST_MULTIPLER) + ((_currentBasicCost * COST_MULTIPLER) 
                                                                          * additionalGoodScoreMultipler)) / _currentMaxProgress;
    }
    
    private void StopProcess(ActiveOrder order)
    {
        ClearProgress();
        _uiView.gameObject.SetActive(false);
    }
    
    private void ActSharpening()
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
            if (_currentProgress >= _currentMaxProgress)
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
    
    private void ChangeStartZonesSize(float sizeDelta)
    {
        _uiView.TargetZoneGreen.sizeDelta = new Vector2(_uiView.TargetZoneGreen.sizeDelta.x + sizeDelta,
            _uiView.TargetZoneGreen.sizeDelta.y);
        _uiView.TargetZoneYellow.sizeDelta = new Vector2(_uiView.TargetZoneYellow.sizeDelta.x + sizeDelta,
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

    private void StartSharpening(int score)
    {
        _currentScore = score;
        _currentProgress = 0;
        _isRunning = true;
    }
    
    private void EndProcess()
    {
        _eventBus.OnSharpeningFinished?.Invoke(_currentScore);
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