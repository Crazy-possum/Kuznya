using System;
using GameCoreModule;
using MAEngine;
using System.Collections.Generic;
using MAEngine.Extention;
using Orders;
using UnityEngine;
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
    private const float GOOD_PROGRESS = 0.02f;
    private const float BAD_PROGRESS = 0.02f;
    private const float PROGRESS_MULTIPLER = 1;
    private const float END_PROCESS_DELAY = 2;
    
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

    private Timer _endProcessTimer;


    [Inject]
    public void Construct(ForgingUIView uiView, ForgingEventBus eventBus,
        GameEventBus gameEventBus, StateEventsBus stateEventsBus,
        OrdersEventBus ordersEventBus, OrderSpritesContainer orderSpritesContainer)
    {
        _uiView = uiView;
        _eventBus = eventBus;
        _gameEventBus = gameEventBus;
        _stateEventsBus = stateEventsBus;
        _ordersEventBus = ordersEventBus;
        _orderSpritesContainer = orderSpritesContainer;
    }


    public void Initialisation()
    {
        ClearProgress();
        _ordersEventBus.OnOrderStarted += StartOrder;
        _gameEventBus.OnCreatePool(PrefabID.UIForgingScoreText);
        UpdateUI();
    }

    public void Cleanup()
    {
        _ordersEventBus.OnOrderStarted -= StartOrder;
        //UnSubscribeWorkingZones();
    }

    public void FixedExecute(float fixedDeltaTime)
    {
        CheckScoreTextsLifetime();
        CheckProgress();
        TryEndProcess();
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
        //_stateEventsBus.OnForgingStateActivate?.Invoke();
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
        ZoneActions(zoneView);
    }
    

    private void ZoneActions(WorkZoneUIView zoneView)
    {
        int score = 0;
        if (zoneView == _currentWorkZoneViews[0])
        {
            score = (int)(GOOD_SCORE * SCORE_MULTIPLER * _currentBasicCost);
            _currentScore += score;
            _currentProgress += GOOD_PROGRESS * PROGRESS_MULTIPLER;
        }
        else
        {
            score = (int)(BAD_SCORE * SCORE_MULTIPLER * _currentBasicCost);
            _currentScore += score;
            _currentProgress += BAD_PROGRESS * PROGRESS_MULTIPLER;
        }
        if (_currentProgress >= 1)
        {
            _currentProgress = 1;
            UpdateUI();
            _endProcessTimer = new Timer(END_PROCESS_DELAY);
        }
        _currentView = zoneView;
        _currentAddingScore = score;
        _gameEventBus.OnObjectSpawnedFromPool += InitializeTextObject;
        _gameEventBus.OnSpawnObjectFromPool?.Invoke(PrefabID.UIForgingScoreText, Vector3.zero);
        UpdateUI();
    }

    private void InitializeTextObject(GameObject textObject, IPool pool)
    {
        textObject.transform.SetParent(_currentView.ScoreRoot);
        ScoreTextView textView = textObject.GetComponent<ScoreTextView>();
        textView.InitializeView(_currentView, LIFETIME);
        textView.Text.text = _currentAddingScore.ToString();
        _currentView.AddTextToList(textView);
        _gameEventBus.OnObjectSpawnedFromPool -= InitializeTextObject;
        _currentView = null;
        _currentAddingScore = 0;

    }

    private void UpdateUI()
    {
        _uiView.ProgressSlider.value = _currentProgress;
        _uiView.ScoreText.text = _currentScore.ToString();
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

    private void EndProcess()
    {
        _eventBus.OnForgingFinished?.Invoke(_currentScore);
        ClearProgress();
        _stateEventsBus.OnResultsStateActivate?.Invoke();
        _uiView.gameObject.SetActive(false);
    }
    
    private void ClearProgress()
    {
        _currentProgress = 0;
        _currentScore = 0;
    }

}
