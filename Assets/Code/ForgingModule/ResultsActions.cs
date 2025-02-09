using GameCoreModule;
using MAEngine;
using UnityEngine;
using Zenject;

public class ResultsActions : IAction, IInitialisation, ICleanUp, IFixedExecute
{
    private const float GOLD_MULTIPLER = 0.1f;
    private const float CONVERTION_SPEED = 0.01f;
    private ResultsUIView _resultsView;
    private ForgingEventBus _eventBus;
    private ResultsEventBus _resultEvents;
    private int _score;
    private int _dynamicScore;
    private int _gold;
    private bool _isCountingResult;
    private float _metaDelta;
    private int _finalGold;

    [Inject]
    public void Construct(ResultsUIView resultsView, ForgingEventBus eventBus,
        ResultsEventBus resultEvents)
    {
        _resultsView = resultsView;
        _eventBus = eventBus;
        _resultEvents = resultEvents;
    }

    public void Initialisation()
    {
        _eventBus.OnForgingFinished += StartCountingResult;
        _resultsView.EndButton.onClick.AddListener(() => StopCounting());
        _resultEvents.OnResultsFinished += ShowDebugResult;
    }

    public void Cleanup()
    {
        _eventBus.OnForgingFinished -= StartCountingResult;
        _resultsView.EndButton.onClick.RemoveListener(() => StopCounting());
        _resultEvents.OnResultsFinished -= ShowDebugResult;
    }

    public void FixedExecute(float fixedDeltaTime)
    {
        if (_isCountingResult)
        {
            if (_dynamicScore != 0)
            {
                int delta = Mathf.RoundToInt(_score * CONVERTION_SPEED);
                if (_dynamicScore > delta)
                {
                    _dynamicScore = _dynamicScore - delta;
                }
                else
                {
                    delta = _dynamicScore;
                    _dynamicScore = 0;
                    _isCountingResult = false;
                    StopCounting();
                }
                float rawDelta = delta * 0.1f;
                int intDelta = Mathf.RoundToInt(delta * GOLD_MULTIPLER);
                float additionDelta = rawDelta - intDelta;

                _metaDelta += additionDelta;
                int roundDelta = 0;
                if (Mathf.Abs(_metaDelta) >= 1)
                {
                    roundDelta = Mathf.RoundToInt(_metaDelta);
                    _metaDelta -= roundDelta;
                }
                _gold = _gold + intDelta + roundDelta;
                UpdateUI();
                if (_dynamicScore == 0)
                {
                    _isCountingResult = false;
                    StopCounting();
                }
            }
        }
    }

    private void StartCountingResult(int score)
    {
        _score = score;
        _dynamicScore = score;
        _gold = 0;
        _isCountingResult = true;
        _finalGold = Mathf.RoundToInt(score * GOLD_MULTIPLER);
        UpdateUI();
    }

    private void StopCounting()
    {
        _isCountingResult = false;
        _gold = _finalGold;
        _dynamicScore = 0;
        UpdateUI();
        _resultEvents.OnResultsFinished?.Invoke(_gold);
        _resultsView.EndButton.onClick.RemoveListener(() => StopCounting());
    }

    private void UpdateUI()
    {
        _resultsView.StaticScoreText.text = _score.ToString();
        _resultsView.DynamicScoreText.text = _dynamicScore.ToString();
        _resultsView.GoldText.text = _gold.ToString();
    }

    private void ShowDebugResult(int gold)
    {
        Debug.Log($"GOLD added : {gold}");
    }

}
