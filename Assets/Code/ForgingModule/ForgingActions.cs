using GameCoreModule;
using MAEngine;
using System.Collections.Generic;
using Orders;
using UnityEngine;
using Zenject;

public class  ForgingActions : IAction, IInitialisation, IFixedExecute, ICleanUp
{
    private const float FIRST_STEP = 0.3f;
    private const float SECOND_STEP = 0.6f;
    private const float LIFETIME = 0.8f;
    private const int GOOD_SCORE = 15;
    private const int BAD_SCORE = 6;
    private const float SCORE_MULTIPLER = 1f;
    private const float GOOD_PROGRESS = 0.02f;
    private const float BAD_PROGRESS = 0.02f;
    private const float PROGRESS_MULTIPLER = 1;
    
    private ForgingUIView _uiView;
    private PrefabsContainer _prefabs;
    private ForgingEventBus _eventBus;
    private GameEventBus _gameEventBus;
    private StateEventsBus _stateEventsBus;
    private OrdersEventBus _ordersEventBus;

    private int _currentZoneIndex;
    private float _currentProgress;
    private int _currentScore;
    private int _currentBasicCost;

    private WorkZoneUIView _currentView;
    private int _currentAddingScore;


    [Inject]
    public void Construct(ForgingUIView uiView, PrefabsContainer prefabs,
        ForgingEventBus eventBus, GameEventBus gameEventBus, StateEventsBus stateEventsBus,
        OrdersEventBus ordersEventBus)
    {
        _uiView = uiView;
        _prefabs = prefabs;
        _eventBus = eventBus;
        _gameEventBus = gameEventBus;
        _stateEventsBus = stateEventsBus;
        _ordersEventBus = ordersEventBus;
    }


    public void Initialisation()
    {
        ClearProgress();
        _ordersEventBus.OnOrderStarted += StartOrder;
        _uiView.Zone1view.ZoneButton.onClick.AddListener(() => Zone1Tiggered());
        _uiView.Zone2view.ZoneButton.onClick.AddListener(() => Zone2Tiggered());
        _uiView.Zone3view.ZoneButton.onClick.AddListener(() => Zone3Tiggered());
        _gameEventBus.OnCreatePool(PrefabID.UIForgingScoreText);
        UpdateUI();
    }

    public void Cleanup()
    {
        _ordersEventBus.OnOrderStarted -= StartOrder;
        _uiView.Zone1view.ZoneButton.onClick.RemoveAllListeners();
        _uiView.Zone2view.ZoneButton.onClick.RemoveAllListeners();
        _uiView.Zone3view.ZoneButton.onClick.RemoveAllListeners();
    }

    public void FixedExecute(float fixedDeltaTime)
    {
        CheckScoreTextsLifetime();
        CheckProgress();
    }
    
    private void StartOrder(ActiveOrder order)
    {
        _currentBasicCost = order.BasicCost;
        _stateEventsBus.OnForgingStateActivate?.Invoke();
    }
    
    private void CheckProgress()
    {
        if (_currentProgress > FIRST_STEP &&
            _currentProgress < SECOND_STEP)
        {
            _currentZoneIndex = 1;
        }
        else if (_currentProgress > SECOND_STEP)
        {
            _currentZoneIndex = 2;
        }
    }

    private void Zone1Tiggered()
    {
        ZoneActions(0);
    }

    private void Zone2Tiggered()
    {
        ZoneActions(1);
    }

    private void Zone3Tiggered()
    {
        ZoneActions(2);
    }

    private void ZoneActions(int zoneIndex)
    {
        int score = 0;
        if (_currentZoneIndex == zoneIndex)
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
            EndProcess();
        }
        WorkZoneUIView view = null;
        if (zoneIndex == 0)
        {
            view = _uiView.Zone1view;
        }
        else if (zoneIndex == 1)
        {
            view = _uiView.Zone2view;
        }
        else if(zoneIndex == 2)
        {
            view = _uiView.Zone3view;
        }
        _currentView = view;
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
        List<ScoreTextView> textsToRemove = new List<ScoreTextView>();
        foreach (ScoreTextView textView in _uiView.Zone1view.ScoreTextList)
        {
            if (textView.LifeTimer.Wait())
            {
                textsToRemove.Add(textView);
            }
        }
        foreach (ScoreTextView textView in _uiView.Zone2view.ScoreTextList)
        {
            if (textView.LifeTimer.Wait())
            {
                textsToRemove.Add(textView);
            }
        }
        foreach (ScoreTextView textView in _uiView.Zone3view.ScoreTextList)
        {
            if (textView.LifeTimer.Wait())
            {
                textsToRemove.Add(textView);
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
        _currentZoneIndex = 0;
        _currentScore = 0;
    }

}
