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
    private const float SCORE_TEXT_LIFETIME = 2f;
    public const float END_TIME = 0.5f;
    
    
    private SharpeningUIView _uiView;
    private ForgingEventBus _eventBus;
    private StateEventsBus _stateEventsBus;
    private OrdersEventBus _ordersEventBus;
    private ProgressionData _progressionData;
    private GameEventBus _gameEventBus;
    private TutorialEventBus _tutorialEventBus;
    private AudioEventBus _audioEventBus;
    private OrderSpritesContainer _orderSpritesContainer;

    private UpgradesMetaData _upgradesMetaData;
    private PlayerMetaData _playerMetaData;
    private int _currentProgress;
    private int _currentScore;
    private bool _isRunning;
    private int _basicAddingScore;
    private int _currentBasicCost;
    private bool _isMovingRight;
    private Vector2 _greenZoneInitialSize;
    private Vector2 _yellowZoneInitialSize;
    private int _currentMaxProgress;
    private ScoreTextView _currentAddingScoreTextView;
    private Timer _endTimer;
    
    [Inject]
    public void Construct(SharpeningUIView uiView, ForgingEventBus forgingEventBus,
        StateEventsBus stateEventsBus, OrdersEventBus ordersEventBus, ProgressionData progressionData,
        GameEventBus gameEventBus, TutorialEventBus tutorialEventBus, AudioEventBus audioEventBus,
        OrderSpritesContainer orderSpritesContainer)
    {
        _uiView = uiView;
        _eventBus = forgingEventBus;
        _stateEventsBus = stateEventsBus;
        _ordersEventBus = ordersEventBus;
        _progressionData = progressionData;
        _gameEventBus = gameEventBus;
        _tutorialEventBus = tutorialEventBus;
        _audioEventBus = audioEventBus;
        _orderSpritesContainer = orderSpritesContainer;
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
        _playerMetaData = _progressionData.PlayerMetaData;
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
            _uiView.HardeningSlider.value += 1.8f;
            if(_uiView.HardeningSlider.value >= _uiView.HardeningSlider.maxValue)
            {
                _isMovingRight = false;
            }
        }
        else
        {
            _uiView.HardeningSlider.value -= 1.8f;
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
        SetItemImage(order);
    }
    
    private void SetItemImage(ActiveOrder order)
    {
        MaterialName materialName = order.Materials[0].Config.MaterialName;
        OrderType orderType = order.OrderType;
        Sprite itemSprite = _orderSpritesContainer.OrderSpritesDict[orderType]
            .ItemSpritesDict[materialName].Stage4Sprite;
        _uiView.ItemImage.sprite = itemSprite;
        _uiView.ItemImage.SetNativeSize();
        //RectTransform imageRect = _hardeningUIView.ItemImage.GetComponent<RectTransform>();
        //imageRect.sizeDelta = new Vector2(imageRect.sizeDelta.x/2, imageRect.sizeDelta.y/2);
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
            _audioEventBus.OnPlaySound?.Invoke(AudioResourceID.Sound_Sharpening);
            bool isInGreenZone = CheckGreenZone();
            bool isInYellowZone = CheckYellowZone();
            int addingScore = 0;
            if (isInGreenZone)
            {
                _currentScore += _basicAddingScore;
                _tutorialEventBus.OnSharpeningGoodHit?.Invoke();
                //Debug.Log($"{_basicAddingScore} added to score");
                addingScore = _basicAddingScore;
            }
            else if (isInYellowZone)
            {
                _currentScore += _basicAddingScore / 2;
                _tutorialEventBus.OnSharpeningBadHit?.Invoke();
                //Debug.Log($"{_basicAddingScore / 2} added to score");
                addingScore = _basicAddingScore / 2;
            }
            else
            {
                _tutorialEventBus.OnSharpeningBadHit?.Invoke();
                //Debug.Log($"Nothing added to score");
            }
            ChangeZonesSize();
            SpawnAddingScoreObject(addingScore);
            UpdateUI();
            _uiView.ItemAnimator.SetTrigger("ActSharpening");
            if (_currentProgress >= _currentMaxProgress)
            {
                _isRunning = false;
                _endTimer = new Timer(END_TIME);
                return;
            }
        }

        if (_endTimer != null)
        {
            if (_endTimer.Wait())
            {
                EndProcess();
                _endTimer = null;
            }
        }
        
        
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
        if (_playerMetaData.Unlocks.IsSharpeningUnlocked)
        {
            _audioEventBus.OnPlayAmbientSound?.Invoke(AudioResourceID.Ambient_SharpStone);
            _audioEventBus.OnPlaySoundLoop?.Invoke(AudioResourceID.Ambient_SharpStone2);
            _tutorialEventBus.OnSharpeningStarted?.Invoke();
            _currentScore = score;
            _currentProgress = 0;
            _isRunning = true;
        }
        else
        {
            _eventBus.OnSharpeningFinished?.Invoke(score);
        }
    }
    
    private void EndProcess()
    {
        _audioEventBus.OnStopAmbientSound?.Invoke();
        _audioEventBus.OnStopSound?.Invoke();
        _eventBus.OnSharpeningFinished?.Invoke(_currentScore);
        ClearProgress();
        if (_playerMetaData.Unlocks.IsFinalizationUnlocked)
        {
            _stateEventsBus.OnFinalizationStateActivate?.Invoke();
        }
        else
        {
            _stateEventsBus.OnResultsStateActivate?.Invoke();
        }
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
        if (_currentAddingScoreTextView != null)
        {
            _currentAddingScoreTextView.RemoveText();
        }
    }
    
    private void UpdateUI()
    {
        _uiView.ScoreText.text = _currentScore.ToString();
    }

}