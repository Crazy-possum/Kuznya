using System.Collections.Generic;
using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using MainGUI;
using Orders;
using Progression;
using UnityEngine;
using Zenject;
using Random = System.Random;


public class FinalizationActions : IAction, IInitialisation, ICleanUp, IFixedExecute
{
    private const float HIGHLIGHT_TIME = 0.5f; 
    public const float MEMORY_TIME = 2f;
    public const float END_TIME = 1f;
    private const float SCORE_TEXT_LIFETIME = 1f;
    
    private FinalizationUIView _uiView;
    private ForgingEventBus _forgingEventBus;
    private StateEventsBus _stateEventsBus;
    private OrdersEventBus _ordersEventBus;
    private AudioEventBus _audioEventBus;
    private ProgressionData _progressionData;
    private GameEventBus _gameEventBus;
    private TutorialEventBus _tutorialEventBus;
    private OrderSpritesContainer _orderSpritesContainer;
    
    private UpgradesMetaData _upgradesMetaData;
    private PlayerMetaData _playerMetaData;
    private System.Random _random;
    private ActiveOrder _activeOrder;
    private int _currentScore;
    private bool _isRunning;
    private int _stagesCount;
    private int _currentStageIndex;
    private int _memoryPointsCount;
    private int _activeMemoryPointsCount;
    private List<ButtonNames> _memoryPointsNames;
    private List<ButtonNames> _currentMemoryPointsNames;
    private bool _isMemoryState;
    private bool _isMemoryStagePassed;
    private ButtonNames _currentHighlightedButtonName;
    private Timer _highlightTimer;
    private Timer _memoryTimer;
    private Timer _endingTimer;
    private ScoreTextView _currentAddingScoreTextView;
    
    [Inject]
    public void Initialize(FinalizationUIView uiView, ForgingEventBus forgingEventBus,
        StateEventsBus stateEventsBus, OrdersEventBus ordersEventBus, AudioEventBus audioEventBus,
        ProgressionData progressionData, GameEventBus gameEventBus, TutorialEventBus tutorialEventBus,
        OrderSpritesContainer orderSpritesContainer)
    {
        _uiView = uiView;
        _forgingEventBus = forgingEventBus;
        _stateEventsBus = stateEventsBus;
        _ordersEventBus = ordersEventBus;
        _audioEventBus = audioEventBus;
        _progressionData = progressionData;
        _gameEventBus = gameEventBus;
        _tutorialEventBus = tutorialEventBus;
        _orderSpritesContainer = orderSpritesContainer;
    }
    
    public void Initialisation()
    {
        _upgradesMetaData = _progressionData.UpgradesMeta;
        _playerMetaData = _progressionData.PlayerMetaData;
        _forgingEventBus.OnSharpeningFinished += StartFinalization;
        _ordersEventBus.OnOrderStarted += StartOrder;
        _ordersEventBus.OnOrderEnded += StopProcess;
        SubscribeButtons();
        _random = new Random();
    }

    public void Cleanup()
    {
        _forgingEventBus.OnSharpeningFinished -= StartFinalization;
        _ordersEventBus.OnOrderStarted -= StartOrder;
        _ordersEventBus.OnOrderEnded -= StopProcess;
        UnSubscribeButtons();
    }
    
    public void FixedExecute(float fixedDeltaTime)
    {
        if (_isRunning)
        {
            if (_isMemoryState)
            {
                _uiView.MemoryTextPanel.SetActive(true);
                _uiView.FinalizeTextPanel.SetActive(false);
                _uiView.BlockerPanel.SetActive(true);
                ActMemory();
            }
            else
            {
                _uiView.MemoryTextPanel.SetActive(false);
                _uiView.FinalizeTextPanel.SetActive(true);
                _uiView.BlockerPanel.SetActive(false);
            }
            
            _uiView.UpdateScore((int)_currentScore);
            CheckEndCondition();
        }

        if (_highlightTimer != null)
        {
            if (_highlightTimer.Wait())
            {
                _uiView.ResetButton(_currentHighlightedButtonName);
                _highlightTimer = null;
                _currentHighlightedButtonName = ButtonNames.NONE;
            }
        }

        if (_endingTimer != null)
        {
            if (_endingTimer.Wait())
            {
                EndProcess();
                _endingTimer = null;
            }
        }
    }

    private void ActMemory()
    {
        if (_memoryTimer != null)
        {
            if (_memoryTimer.Wait())
            {
                if (_activeMemoryPointsCount >= _memoryPointsCount)
                {
                    _isMemoryState = false;
                    _activeMemoryPointsCount = 0;
                    return;
                }
                int randomIndex = _random.Next(0, _memoryPointsNames.Count);
                ButtonNames buttonName = _memoryPointsNames[randomIndex];
                _currentMemoryPointsNames.Add(buttonName);
                _activeMemoryPointsCount++;
                _uiView.HighlightGreen(buttonName);
                _currentHighlightedButtonName = buttonName;
                _highlightTimer = new Timer(HIGHLIGHT_TIME);

            }
        }
        else
        {
            _activeMemoryPointsCount = 0;
            _memoryTimer = new Timer(MEMORY_TIME);
        }
    }

    private void SubscribeButtons()
    {
        for(int i = 0; i < _uiView.MemoryButtons.Length; i++)
        {
            ButtonView memoryButton = _uiView.MemoryButtons.GetValueByIndex(i);
            ButtonNames buttonName = _uiView.MemoryButtons.GetKeyByIndex(i);
            _uiView.ResetButton(buttonName);
            memoryButton.Button.onClick.AddListener( () => ActFinalization(buttonName));
        }
    }
    
    private void UnSubscribeButtons()
    {
        for(int i = 0; i < _uiView.MemoryButtons.Length; i++)
        {
            ButtonView memoryButton = _uiView.MemoryButtons.GetValueByIndex(i);
            memoryButton.Button.onClick.RemoveAllListeners();
        }
    }
    
    private void ActFinalization(ButtonNames buttonName)
    {
        if (_currentHighlightedButtonName != ButtonNames.NONE)
        {
            _uiView.ResetButton(_currentHighlightedButtonName);
            _highlightTimer = null;
        }
        _audioEventBus.OnPlaySound?.Invoke(AudioResourceID.Sound_FinalizationHit1);
        _audioEventBus.OnPlaySound2?.Invoke(AudioResourceID.Sound_FinalizationHit2);
        if (_currentMemoryPointsNames[_activeMemoryPointsCount] == buttonName)
        {
            _tutorialEventBus.OnFinalizationGoodHit?.Invoke();
            _uiView.HighlightGreen(buttonName);
        }
        else
        {
            _tutorialEventBus.OnFinalizationBadHit?.Invoke();
            _uiView.HighlightRed(buttonName);
            _isMemoryStagePassed = false;
            _activeMemoryPointsCount = _memoryPointsCount;
        }
        _activeMemoryPointsCount++;
        _currentHighlightedButtonName = buttonName;
        _highlightTimer = new Timer(HIGHLIGHT_TIME);
        
        if (_activeMemoryPointsCount >= _memoryPointsCount)
        {
            if (_isMemoryStagePassed)
            {
                AddScore();
            }

            _activeMemoryPointsCount = 0;
            _currentMemoryPointsNames = new List<ButtonNames>();
            _memoryTimer = new Timer(MEMORY_TIME);
            _isMemoryStagePassed = true;
            _isMemoryState = true;
            _currentStageIndex++;
        }
    }

    private void AddScore()
    {
        int stagesCount = _stagesCount;
        if (stagesCount == 0)
        {
            stagesCount = 1;
        }
        float addingScore = (_activeOrder.BasicCost * 50) / stagesCount;
        _currentScore += (int)addingScore;
        SpawnAddingScoreObject((int)addingScore);
        
    }
    
    private void SpawnAddingScoreObject(int addingScore)
    {
        SpawnScoreTextView();
        _currentAddingScoreTextView.Text.text = $"+ {addingScore}";
    }
    
    private void SpawnScoreTextView()
    {
        if (_currentAddingScoreTextView != null)
        {
            _currentAddingScoreTextView = null;
        }
        GameObjectSpawnCallback callback = new GameObjectSpawnCallback();
        _gameEventBus.OnSpawnObjectWithoutRoot?.Invoke(PrefabID.UIForgingScoreText, Vector3.zero, callback);
        InitializeTextObject(callback.SpawnedObject);
    }
    
    private void InitializeTextObject(GameObject textObject)
    {
        textObject.transform.SetParent(_uiView.ScoreRoot);
        ScoreTextView textView = textObject.GetComponent<ScoreTextView>();
        textView.TextRectTransform.anchoredPosition = _uiView.ScoreRoot.rect.center;
        textView.InitializeView(SCORE_TEXT_LIFETIME);
        _currentAddingScoreTextView = textView;

    }

    private void CheckEndCondition()
    {
        if (_stagesCount == 0)
        {
            AddScore();
            EndProcess();
        }
        if (_currentStageIndex == _stagesCount)
        {
            _isRunning = false;
            _endingTimer = new Timer(END_TIME);
        }
    }

    private void StartOrder(ActiveOrder order)
    {
        _activeOrder = order;
        _memoryPointsNames = new List<ButtonNames>();
        _currentMemoryPointsNames = new List<ButtonNames>();
        OrderType orderType = order.OrderType;
        InitializeMemoryGame(orderType);
        _uiView.ItemImage.sprite = _orderSpritesContainer.OrderSpritesDict[order.OrderType]
            .ItemSpritesDict[order.Materials[0].Config.MaterialName].OrderSprite;;
    }

    private void InitializeMemoryGame(OrderType orderType)
    {
        switch (orderType)
        {
            case OrderType.Sword:
                _stagesCount = 3;
                _memoryPointsCount = 2;
                _memoryPointsNames.Add(ButtonNames.Button_1);
                _memoryPointsNames.Add(ButtonNames.Button_2);
                _memoryPointsNames.Add(ButtonNames.Button_3);
                _memoryPointsNames.Add(ButtonNames.Button_4);
                _memoryPointsNames.Add(ButtonNames.Button_6);
                _memoryPointsNames.Add(ButtonNames.Button_7);
                break;
            case OrderType.Axe:
                _stagesCount = 2;
                _memoryPointsCount = 2;
                _memoryPointsNames.Add(ButtonNames.Button_2);
                _memoryPointsNames.Add(ButtonNames.Button_3);
                _memoryPointsNames.Add(ButtonNames.Button_6);
                _memoryPointsNames.Add(ButtonNames.Button_7);
                _memoryPointsNames.Add(ButtonNames.Button_10);
                break;
            case OrderType.Sickle:
                _stagesCount = 1;
                _memoryPointsCount = 2;
                _memoryPointsNames.Add(ButtonNames.Button_1);
                _memoryPointsNames.Add(ButtonNames.Button_2);
                _memoryPointsNames.Add(ButtonNames.Button_3);
                _memoryPointsNames.Add(ButtonNames.Button_4);
                break;
            case OrderType.Scythe:
                _stagesCount = 3;
                _memoryPointsCount = 3;
                _memoryPointsNames.Add(ButtonNames.Button_3);
                _memoryPointsNames.Add(ButtonNames.Button_4);
                _memoryPointsNames.Add(ButtonNames.Button_6);
                _memoryPointsNames.Add(ButtonNames.Button_7);
                _memoryPointsNames.Add(ButtonNames.Button_11);
                break;
            case OrderType.Spear:
                _stagesCount = 1;
                _memoryPointsCount = 4;
                _memoryPointsNames.Add(ButtonNames.Button_2);
                _memoryPointsNames.Add(ButtonNames.Button_3);
                _memoryPointsNames.Add(ButtonNames.Button_6);
                _memoryPointsNames.Add(ButtonNames.Button_7);
                _memoryPointsNames.Add(ButtonNames.Button_10);
                _memoryPointsNames.Add(ButtonNames.Button_11);
                break;
            case OrderType.Knife:
                _stagesCount = 1;
                _memoryPointsCount = 2;
                _memoryPointsNames.Add(ButtonNames.Button_1);
                _memoryPointsNames.Add(ButtonNames.Button_2);
                _memoryPointsNames.Add(ButtonNames.Button_3);
                _memoryPointsNames.Add(ButtonNames.Button_4);
                break;
            case OrderType.Saw:
                _stagesCount = 2;
                _memoryPointsCount = 4;
                _memoryPointsNames.Add(ButtonNames.Button_1);
                _memoryPointsNames.Add(ButtonNames.Button_4);
                _memoryPointsNames.Add(ButtonNames.Button_5);
                _memoryPointsNames.Add(ButtonNames.Button_8);
                _memoryPointsNames.Add(ButtonNames.Button_9);
                _memoryPointsNames.Add(ButtonNames.Button_12);
                break;
            case OrderType.Dagger:
                _stagesCount = 1;
                _memoryPointsCount = 2;
                _memoryPointsNames.Add(ButtonNames.Button_1);
                _memoryPointsNames.Add(ButtonNames.Button_2);
                _memoryPointsNames.Add(ButtonNames.Button_3);
                _memoryPointsNames.Add(ButtonNames.Button_4);
                break;
            case OrderType.Wheel:
                _stagesCount = 3;
                _memoryPointsCount = 2;
                _memoryPointsNames.Add(ButtonNames.Button_3);
                _memoryPointsNames.Add(ButtonNames.Button_5);
                _memoryPointsNames.Add(ButtonNames.Button_6);
                _memoryPointsNames.Add(ButtonNames.Button_10);
                break;
            default:
                _stagesCount = 0;
                _memoryPointsCount = 0;
                break;
        }
    }

    private void StartFinalization(int score)
    {
        if (_playerMetaData.Unlocks.IsFinalizationUnlocked)
        {
            _tutorialEventBus.OnFinalizationStarted?.Invoke();
            if (_isRunning) return;
            _isRunning = true;
            _currentScore = score;
            _uiView.UpdateScore((int)_currentScore);
            _isMemoryState = true;
            _isMemoryStagePassed = true;

        }
        else
        {
            _forgingEventBus.OnFinalizationFinished?.Invoke(score);
        }
    }
    
    private void StopProcess(ActiveOrder order)
    {
        ClearProgress();
        _uiView.gameObject.SetActive(false);
    }

    private void EndProcess()
    {
        _forgingEventBus.OnFinalizationFinished?.Invoke((int)_currentScore);
        ClearProgress();
        _stateEventsBus.OnResultsStateActivate?.Invoke();
        _uiView.gameObject.SetActive(false);
    }
    
    private void ClearProgress()
    {
        _isRunning = false;
        _currentScore = 0;
        _currentStageIndex = 0;
        _memoryPointsCount = 0;
        _activeMemoryPointsCount = 0;
        if (_currentAddingScoreTextView != null)
        {
            _currentAddingScoreTextView.RemoveText();
        }
        if(_currentHighlightedButtonName != ButtonNames.NONE)
        {
            _uiView.ResetButton(_currentHighlightedButtonName);
            _currentHighlightedButtonName = ButtonNames.NONE;
        }
    }
}