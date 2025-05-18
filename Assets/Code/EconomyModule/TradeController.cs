using System;
using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using Orders;
using Progression;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Economy
{
    public class TradeController : IAction, IInitialisation, ICleanUp, IFixedExecute
    {
        private TradeView _tradeView;
        private EconomyEventBus _economyEventBus;
        private StateEventsBus _stateEventsBus;
        private ProgressionData _progressionData;
        private TutorialEventBus _tutorialEventBus;

        private ActiveOrder _currentOrder;
        private float _currentMultipler;
        private float _tradingTimeLeft;
        private bool _isTrading;
        private float _currentTradeValue;
        private Timer _tradeTickTimer;
        private TradeState _tradeState;
        private Random _random;
        private UpgradesMetaData _upgradesMetaData;
        private PlayerMetaData _playerMetaData;
        

        [Inject]
        public void Construct(TradeView tradeView, EconomyEventBus economyEventBus,
            StateEventsBus stateEventsBus, ProgressionData progressionData,
            TutorialEventBus tutorialEventBus)
        {
            _tradeView = tradeView;
            _economyEventBus = economyEventBus;
            _stateEventsBus = stateEventsBus;
            _progressionData = progressionData;
            _tutorialEventBus = tutorialEventBus;
        }
        
        public void Initialisation()
        {
            _upgradesMetaData = _progressionData.UpgradesMeta;
            _playerMetaData = _progressionData.PlayerMetaData;
            _tradeView.TradeButton.onClick.AddListener(StartTrading);
            _tradeView.ConfirmButton.onClick.AddListener(SubmitOrder);
            _tradeView.TradeClickerButton.onClick.AddListener(AddTradingScore);
            _economyEventBus.OnOrderSubmited += SetCurrentOrder;
            _stateEventsBus.OnTradeStateActivate += SetTradeButtonState;
            _tradeView.TradeButton.gameObject.SetActive(false);
            _random = new Random();
        }

        private void SetTradeButtonState()
        {
            if (_playerMetaData.Unlocks.IsTradeUnlocked)
            {
                _tutorialEventBus.OnTradeStarted?.Invoke();
                _tradeView.TradeButton.gameObject.SetActive(true);
            }
            else
            {
                _tradeView.TradeButton.gameObject.SetActive(false);
            }
        }

        public void Cleanup()
        {
            _tradeView.TradeButton.onClick.RemoveListener(StartTrading);
            _tradeView.ConfirmButton.onClick.RemoveListener(SubmitOrder);
            _tradeView.TradeClickerButton.onClick.RemoveListener(AddTradingScore);
            _economyEventBus.OnOrderSubmited -= SetCurrentOrder;
        }
        
        public void FixedExecute(float fixedDeltaTime)
        {
            if (_isTrading)
            {
                if (_tradeTickTimer.Wait())
                {
                    _tradeState = TradeAction(fixedDeltaTime);
                }
                int timeMultipler = 1;
                if (_tradeState == TradeState.WhiteZone)
                {
                    timeMultipler = 2;
                }
                _tradingTimeLeft -= fixedDeltaTime * timeMultipler;
                
                if (_tradingTimeLeft <= 0)
                {
                    _isTrading = false;
                    SetTradedState(true);
                    _tradeView.HideTradePanel();
                    _tutorialEventBus.OnTradeFinished?.Invoke();
                }
                UpdateUI();
                
                float tradeValueDelta = 0;
                float upgradeMultipler = 0;
                if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.TradeDiplomacy))
                {
                    upgradeMultipler = _upgradesMetaData.Upgrades[UpgradeName.TradeDiplomacy].GetUpgradeData();
                }
                if (_tradeState == TradeState.WhiteZone)
                {
                    tradeValueDelta = _random.Next(-4, 16);
                }
                else if (_tradeState == TradeState.YellowZone)
                {
                    tradeValueDelta = _random.Next(3, 12);
                }
                else if (_tradeState == TradeState.GreenZone)
                {
                    tradeValueDelta = _random.Next(1, 5);
                }
                else if (_tradeState == TradeState.RedZone)
                {
                    tradeValueDelta = _random.Next(1, 3);
                }
                tradeValueDelta = (tradeValueDelta / 100f) - ((tradeValueDelta / 100f) * upgradeMultipler);
                _currentTradeValue -= tradeValueDelta;
            }
        }

        private TradeState TradeAction(float fixedDeltaTime)
        {
            TradeState state = TradeState.NONE;
            if (_currentTradeValue >= 55 && _currentTradeValue <= 60)
            {
                _currentMultipler += 0.08f;
                state = TradeState.GreenZone;
            }
            else if (_currentTradeValue >= 61 && _currentTradeValue <= 70)
            {
                _currentMultipler -= 0.02f;
                state = TradeState.YellowZone;
            }
            else if(_currentTradeValue <= 54 && _currentTradeValue >= 45)
            {
                _currentMultipler -= 0.02f;
                state = TradeState.YellowZone;
            }
            else if (_currentTradeValue > 70)
            {
                _currentMultipler -= 0.12f;
                state = TradeState.RedZone;
            }
            else
            {
                _currentMultipler -= 0.04f;
                state = TradeState.WhiteZone;
            }
            _currentMultipler = Mathf.Clamp(_currentMultipler, -0.25f, 1.01f);
            return state;
        }

        private void AddTradingScore()
        {
            if (_tradeState == TradeState.WhiteZone ||
                _tradeState == TradeState.YellowZone)
            {
                _currentTradeValue += 5;
            }
            else if (_tradeState == TradeState.GreenZone)
            {
                _tutorialEventBus.OnTradeGreenZone?.Invoke();
                _currentTradeValue += 3;
            }
            else if (_tradeState == TradeState.RedZone)
            {
                _currentTradeValue += 6;
            }
            
            _currentTradeValue = Mathf.Clamp(_currentTradeValue,0, _tradeView.TradeSlider.maxValue);
        }
        
        private void SubmitOrder()
        {
            int reward = GetRewardValue();
            _economyEventBus.OnAddMoney?.Invoke((int)(reward));
            _stateEventsBus.OnOrdersStateActivate?.Invoke();
            _tradeView.AdditionRewardText.gameObject.SetActive(false);
            _tradeView.RewardText.gameObject.SetActive(false);
        }

        private void StartTrading()
        {
            _tutorialEventBus.OnTradeScreenOpened?.Invoke();
            _isTrading = true;
            _tradingTimeLeft = 20;
            _tradeTickTimer = new Timer(1.5f);
            _tradeState = TradeState.WhiteZone;
            _currentTradeValue = 0;
            _tradeView.ShowTradePanel();
            _tradeView.AdditionRewardText.gameObject.SetActive(true);
            _tradeView.RewardText.gameObject.SetActive(true);
        }
        
        private void SetCurrentOrder(ActiveOrder order)
        {
            _currentOrder = order;
            SetTradedState(false);
            _currentMultipler = 0;
            _tradeView.ClientImage.sprite = order.ClientIcon;
            _tradeView.ClientName.text = order.ClientName;
            UpdateUI();
        }

        private void SetTradedState(bool wasTraded)
        {
            _tradeView.TradeButton.interactable = !wasTraded;
        }

        private void UpdateUI()
        {
            int basicCost = _currentOrder.BasicCost * 50;
            float tradeMultipler = 0;
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.TradeSpeechcraft))
            {
                tradeMultipler = _upgradesMetaData.Upgrades[UpgradeName.TradeSpeechcraft].GetUpgradeData();
            }

            int addingCost = (int)(basicCost * _currentMultipler) +
                             (int)Mathf.Ceil((basicCost * _currentMultipler) * tradeMultipler);
            int reward = _currentOrder.Reward + addingCost;
            _tradeView.FullRewardText.text = reward.ToString();
            _tradeView.RewardText.text = _currentOrder.Reward.ToString();
            string addingCostString = ""; 
            if (addingCost >= 0)
            {
                addingCostString = $" + {addingCost}";
                _tradeView.AdditionRewardText.color = _tradeView.GoodColor;
            }
            else
            {
                addingCostString = $" - {Mathf.Abs(addingCost)}";
                _tradeView.AdditionRewardText.color = _tradeView.BadColor;
            }
            _tradeView.AdditionRewardText.text = addingCostString;
            if (addingCost > 0)
            {
                _tradeView.Multipler.text = $"+ {addingCost}";
            }
            else
            {
                _tradeView.Multipler.text = $"- {Mathf.Abs(addingCost)}";
            }
            
            TimeSpan timeLeft = TimeSpan.FromSeconds(_tradingTimeLeft);
            _tradeView.Timer.text = timeLeft.ToString("mm' : 'ss");
            _tradeView.TradeSlider.value = _currentTradeValue;
        }

        private int GetRewardValue()
        {
            int basicCost = _currentOrder.BasicCost * 50;
            float tradeMultipler = 0;
            if (_upgradesMetaData.Upgrades.IsContainsKey(UpgradeName.TradeSpeechcraft))
            {
                tradeMultipler = _upgradesMetaData.Upgrades[UpgradeName.TradeSpeechcraft].GetUpgradeData();
            }
            int addingCost = (int)(basicCost * _currentMultipler) + 
                             (int)Mathf.Ceil((basicCost * _currentMultipler) * tradeMultipler);
            return _currentOrder.Reward + addingCost;
        }
    }
}