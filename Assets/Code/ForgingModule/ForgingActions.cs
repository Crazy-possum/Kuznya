using System;
using GameCoreModule;
using MAEngine;
using System.Collections.Generic;
using MAEngine.Extention;
using Orders;
using Progression;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class  ForgingActions : IAction, IInitialisation, IFixedExecute, ICleanUp
{
    private const float FIRST_STEP = 0.25f;
    private const float SECOND_STEP = 0.5f;
    private const float THIRD_STEP = 0.75f;
    private const float FOURTH_STEP = 1f;
    private const float LIFETIME = 0.8f;
    private const int GOOD_SCORE = 15;
    private const int BAD_SCORE = 6;
    private const float SCORE_MULTIPLER = 1f;
    private const float PROGRESS_VALUE = 0.02f;
    private const float PROGRESS_MULTIPLER = 1;
    private const float END_PROCESS_DELAY = 2;
    private const float COOLING_STEP_DURATION = 0.8f;
    private const float COOLING_STEP_VALUE_DELTA = 0.01f;
    
    private ForgingUIView _uiView;
    private ForgingEventBus _eventBus;
    private GameEventBus _gameEventBus;
    private StateEventsBus _stateEventsBus;
    private OrdersEventBus _ordersEventBus;
    private OrderSpritesContainer _orderSpritesContainer;

    private float _currentProgress;
    private int _currentScore;
    private int _currentBasicCost;

    private WorkZoneUIView _currentView;
    private int _currentAddingScore;
    private ItemSprites _itemSprites;
    private ForgingStepsView _currentForgingSteps;
    private List<WorkZoneUIView> _currentWorkZoneViews;
    private ProgressionData _progressionData;

    private Timer _endProcessTimer;
    private float _currentSmeltingBonus;
    private Timer _coolingTimer;
    private bool _isHeated;
    private UpgradesMetaData _upgradesMetaData;


    [Inject]
    public void Construct(ForgingUIView uiView, ForgingEventBus eventBus,
        GameEventBus gameEventBus, StateEventsBus stateEventsBus,
        OrdersEventBus ordersEventBus, OrderSpritesContainer orderSpritesContainer,
        ProgressionData progressionData)
    {
        _uiView = uiView;
        _eventBus = eventBus;
        _gameEventBus = gameEventBus;
        _stateEventsBus = stateEventsBus;
        _ordersEventBus = ordersEventBus;
        _orderSpritesContainer = orderSpritesContainer;
        _progressionData = progressionData;
    }


    public void Initialisation()
    {
        ClearProgress();
        _upgradesMetaData = _progressionData.UpgradesMeta;
        _ordersEventBus.OnOrderStarted += StartOrder;
        _ordersEventBus.OnOrderEnded += StopProcess;
        _stateEventsBus.OnForgingStateActivate += SetForgingBonus;
        _uiView.SmeltingButton.Button.onClick.AddListener(GoToSmelting);
        UpdateUI();
    }

    public void Cleanup()
    {
        _ordersEventBus.OnOrderStarted -= StartOrder;
        _ordersEventBus.OnOrderEnded -= StopProcess;
        _stateEventsBus.OnForgingStateActivate -= SetForgingBonus;
        _uiView.SmeltingButton.Button.onClick.RemoveListener(GoToSmelting);
    }

    public void FixedExecute(float fixedDeltaTime)
    {
        CheckScoreTextsLifetime();
        CheckProgress();
        TryEndProcess();
        ActCooling();
    }

    private void ActCooling()
    {
        if (_coolingTimer != null)
        {
            if (_coolingTimer.Wait())
            {
                _currentSmeltingBonus -= COOLING_STEP_VALUE_DELTA;
                if (_currentSmeltingBonus <= 0)
                {
                    _currentSmeltingBonus = 0;
                    _isHeated = false;
                    _coolingTimer = null;
                }
                UpdateUI();
            }
        }
    }
    
    private void SetForgingBonus(float forgingBonus)
    {
        _currentSmeltingBonus = forgingBonus;
        _coolingTimer = new Timer(COOLING_STEP_DURATION);
        _isHeated = true;
        UpdateUI();
    }

    private void GoToSmelting()
    {
        _coolingTimer = null;
        _stateEventsBus.OnSmeltingStateActivate?.Invoke();
    }

    private void TryEndProcess()
    {
        if (_endProcessTimer != null)
        {
            if (_endProcessTimer.Wait())
            {
                _endProcessTimer = null;
                EndProcess();
            }
        }
    }

    private void StartOrder(ActiveOrder order)
    {
        _currentBasicCost = order.BasicCost;
        _itemSprites = _orderSpritesContainer.OrderSpritesDict[order.OrderType];
        _uiView.ItemImage.sprite = _itemSprites.Stage0Sprite;
        _currentForgingSteps = _uiView.StepsDict[order.OrderType];
        _uiView.ItemImage.SetNativeSize();
        _stateEventsBus.OnSmeltingStateActivate?.Invoke();
        UpdateForgingSteps(0);
    }

    private void UpdateForgingSteps(int stepIndex)
    {
        UnSubscribeWorkingZones();
        switch (stepIndex)
        {
            case 0:
                _currentWorkZoneViews = _currentForgingSteps.Step1WorkZones;
                break;
            case 1:
                _currentWorkZoneViews = _currentForgingSteps.Step2WorkZones;
                break;
            case 2:
                _currentWorkZoneViews = _currentForgingSteps.Step3WorkZones;
                break;
            case 3:
                _currentWorkZoneViews = _currentForgingSteps.Step4WorkZones;
                break;
            default:
                return;
        }
        SubscribeWorkingZones();
    }

    private void UnSubscribeWorkingZones()
    {
        if (_currentWorkZoneViews == null)
        {
            return;
        }

        if (_currentWorkZoneViews.Count != 0)
        {
            foreach (WorkZoneUIView workZoneView in _currentWorkZoneViews)
            {
                workZoneView.gameObject.SetActive(false);
                workZoneView.ZoneButton.onClick.RemoveAllListeners();
            }
        }

    }

    private void SubscribeWorkingZones()
    {
        if (_currentWorkZoneViews.Count != 0)
        {
            int index = 0;
            foreach (WorkZoneUIView workZoneView in _currentWorkZoneViews)
            {
                workZoneView.gameObject.SetActive(true);
                workZoneView.ZoneButton.onClick.AddListener(() => ZoneTiggered(workZoneView));
                index++;
            }
        }
    }

    private void CheckProgress()
    {
        if (_currentProgress > FIRST_STEP &&
            _currentProgress < SECOND_STEP)
        {
            _uiView.ItemImage.sprite = _itemSprites.Stage1Sprite;
            _uiView.ItemImage.SetNativeSize();
            UpdateForgingSteps(1);
        }
        else if (_currentProgress > SECOND_STEP &&
                 _currentProgress < THIRD_STEP)
        {
            _uiView.ItemImage.sprite = _itemSprites.Stage2Sprite;
            _uiView.ItemImage.SetNativeSize();
            UpdateForgingSteps(2);
        }
        else if (_currentProgress > THIRD_STEP &&
                 _currentProgress < FOURTH_STEP)
        {
            _uiView.ItemImage.sprite = _itemSprites.Stage3Sprite;
            _uiView.ItemImage.SetNativeSize();
            UpdateForgingSteps(3);
        }
        else if (_currentProgress >= FOURTH_STEP &&
                 _currentProgress <= 1)
        {
            _uiView.ItemImage.sprite = _itemSprites.Stage4Sprite;
            _uiView.ItemImage.SetNativeSize();
            UnSubscribeWorkingZones();
        }
    }

    private void ZoneTiggered(WorkZoneUIView zoneView)
    {
        if (_isHeated)
        {
            ZoneActions(zoneView);
        }
    }
    

    private void ZoneActions(WorkZoneUIView zoneView)
    {
        int score = 0;
        float progressMultipler = 0;
        float additionalGoodScoreMultipler = 0;
        float additionalBadScoreMultipler = 0;
        if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.ForgingEfficiency))
        {
            progressMultipler = _upgradesMetaData.Upgrades[UpgradeName.ForgingEfficiency].GetUpgradeData();
        }
        if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.ForgingScore))
        {
            additionalGoodScoreMultipler = _upgradesMetaData.Upgrades[UpgradeName.ForgingScore].GetUpgradeData();
        }
        if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.ForgingMistakeScore))
        {
            additionalBadScoreMultipler = _upgradesMetaData.Upgrades[UpgradeName.ForgingMistakeScore].GetUpgradeData();
        }

        float additionalProgerss = PROGRESS_VALUE * progressMultipler;
        float additionalGoodScore = Mathf.Ceil(GOOD_SCORE * additionalGoodScoreMultipler);
        float additionalBadScore = Mathf.Ceil(BAD_SCORE * additionalBadScoreMultipler);
        
        if (zoneView == _currentWorkZoneViews[0])
        {
            score = (int)(GOOD_SCORE * SCORE_MULTIPLER * _currentBasicCost);
            score = score + (int)((score + additionalGoodScore) * _currentSmeltingBonus);
            _currentScore += score;
            _currentProgress += (PROGRESS_VALUE + additionalProgerss) * PROGRESS_MULTIPLER;
        }
        else
        {
            score = (int)(BAD_SCORE * SCORE_MULTIPLER * _currentBasicCost);
            score = score + (int)((score + additionalBadScore) * _currentSmeltingBonus);
            _currentScore += score;
            _currentProgress += (PROGRESS_VALUE + additionalProgerss) * PROGRESS_MULTIPLER;
        }
        if (_currentProgress >= 1)
        {
            _currentProgress = 1;
            UpdateUI();
            _endProcessTimer = new Timer(END_PROCESS_DELAY);
        }
        _currentView = zoneView;
        _currentAddingScore = score;
        GameObjectSpawnCallback callback = new GameObjectSpawnCallback();
        _gameEventBus.OnSpawnObjectWithoutRoot?.Invoke(PrefabID.UIForgingScoreText, Vector3.zero, callback);
        InitializeTextObject(callback.SpawnedObject);
        UpdateUI();
    }

    private void InitializeTextObject(GameObject textObject)
    {
        textObject.transform.SetParent(_currentView.ScoreRoot);
        ScoreTextView textView = textObject.GetComponent<ScoreTextView>();
        
        textView.TextRectTransform.anchoredPosition = _currentView.ScoreRoot.rect.center;
        textView.InitializeView(_currentView, LIFETIME);
        textView.Text.text = _currentAddingScore.ToString();
        _currentView.AddTextToList(textView);
        _currentView = null;
        _currentAddingScore = 0;

    }

    private void UpdateUI()
    {
        _uiView.ProgressSlider.value = _currentProgress;
        _uiView.ScoreText.text = _currentScore.ToString();
        _uiView.SmeltingBonusText.text = $"+{(int)(_currentSmeltingBonus * 100f)}%";
    }

    private void CheckScoreTextsLifetime()
    {
        if (_currentWorkZoneViews == null)
        {
            return;
        }
        List<ScoreTextView> textsToRemove = new List<ScoreTextView>();
        foreach (WorkZoneUIView workZoneView in _currentWorkZoneViews)
        {
            foreach (ScoreTextView textView in workZoneView.ScoreTextList)
            {
                if (textView.LifeTimer.Wait())
                {
                    textsToRemove.Add(textView);
                }
            }
        }
        foreach (ScoreTextView textView in textsToRemove)
        {
            textView.RemoveText();
        }
    }

    private void StopProcess(ActiveOrder order = null)
    {
        ClearProgress();
        _uiView.gameObject.SetActive(false);
    }

    private void EndProcess()
    {
        _eventBus.OnForgingFinished?.Invoke(_currentScore);
        ClearProgress();
        _stateEventsBus.OnSharpeningStateActivate?.Invoke();
        _uiView.gameObject.SetActive(false);
    }
    
    private void ClearProgress()
    {
        _currentProgress = 0;
        _currentScore = 0;
    }

}
