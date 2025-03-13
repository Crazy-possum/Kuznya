using System;
using GameCoreModule;
using MAEngine;
using Orders;
using UnityEngine;
using Zenject;

namespace Economy
{
    public class TradeController : IAction, IInitialisation, ICleanUp, IFixedExecute
    {
        private TradeView _tradeView;
        private EconomyEventBus _economyEventBus;
        private StateEventsBus _stateEventsBus;

        private ActiveOrder _currentOrder;
        private bool _wasTraded;
        private float _currentMultipler;
        private float _tradingTimeLeft;
        private bool _isTrading;
        private int _tradeStateId;
        private float _currentTradeValue;
        

        [Inject]
        public void Construct(TradeView tradeView, EconomyEventBus economyEventBus,
            StateEventsBus stateEventsBus)
        {
            _tradeView = tradeView;
            _economyEventBus = economyEventBus;
            _stateEventsBus = stateEventsBus;
        }
        
        public void Initialisation()
        {
            _tradeView.TradeButton.onClick.AddListener(StartTrading);
            _tradeView.ConfirmButton.onClick.AddListener(SubmitOrder);
            _tradeView.TradeClickerButton.onClick.AddListener(AddTradingScore);
            _economyEventBus.OnOrderSubmited += SetCurrentOrder;
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
                if (_currentTradeValue >= 55 && _currentTradeValue <= 60)
                {
                    _currentMultipler += 0.08f;
                    _tradeStateId = 2;
                }
                else if (_currentTradeValue >= 61 && _currentTradeValue <= 70)
                {
                    _currentMultipler += 0.02f;
                    _tradeStateId = 1;
                }
                else if(_currentTradeValue <= 54 && _currentTradeValue >= 45)
                {
                    _currentMultipler += 0.02f;
                    _tradeStateId = 1;
                }
                else
                {
                    _currentMultipler -= 0.02f;
                    _tradeStateId = 0;
                }

                _currentMultipler = Mathf.Clamp(_currentMultipler, 1, 3);
                
                int timeMultipler = 1;
                if (_tradeStateId == 0)
                {
                    timeMultipler = 2;
                }
                _tradingTimeLeft -= fixedDeltaTime * timeMultipler;
                
                if (_tradingTimeLeft <= 0)
                {
                    _isTrading = false;
                    SetTradedState(true);
                    _tradeView.HideTradePanel();
                }
                UpdateUI();
                _currentTradeValue -= 0.05f;
            }
        }
        
        private void AddTradingScore()
        {
            _currentTradeValue += 5;
        }
        
        private void SubmitOrder()
        {
            _economyEventBus.OnAddMoney?.Invoke((int)(_currentOrder.Reward * _currentMultipler));
            _stateEventsBus.OnOrdersStateActivate?.Invoke();
        }

        private void StartTrading()
        {
            _isTrading = true;
            _tradingTimeLeft = 20;
            _tradeStateId = 0;
            _currentTradeValue = 0;
            _tradeView.ShowTradePanel();
        }
        
        private void SetCurrentOrder(ActiveOrder order)
        {
            _currentOrder = order;
            SetTradedState(false);
            _currentMultipler = 1;
            UpdateUI();
        }

        private void SetTradedState(bool wasTraded)
        {
            _wasTraded = wasTraded;
            _tradeView.TradeButton.interactable = !wasTraded;
        }

        private void UpdateUI()
        {
            int reward = (int)(_currentOrder.Reward * _currentMultipler);
            _tradeView.RewardText.text = reward.ToString();
            _tradeView.Multipler.text = _currentMultipler.ToString();
            TimeSpan timeLeft = TimeSpan.FromSeconds(_tradingTimeLeft);
            _tradeView.Timer.text = timeLeft.ToString("mm' : 'ss");
            _tradeView.TradeSlider.value = _currentTradeValue;
        }


    }
}