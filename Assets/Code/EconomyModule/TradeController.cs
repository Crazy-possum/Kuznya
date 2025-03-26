using System;
using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using Orders;
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

        private ActiveOrder _currentOrder;
        private float _currentMultipler;
        private float _tradingTimeLeft;
        private bool _isTrading;
        private float _currentTradeValue;
        private Timer _tradeTickTimer;
        private TradeState _tradeState;
        private Random _random;
        

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
            _random = new Random();
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
                }
                UpdateUI();
                
                float tradeValueDelta = 0;
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
                tradeValueDelta = tradeValueDelta / 100f;
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
            _currentMultipler = Mathf.Clamp(_currentMultipler, 1, 1.3f);
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
            _economyEventBus.OnAddMoney?.Invoke((int)(_currentOrder.Reward * _currentMultipler));
            _stateEventsBus.OnOrdersStateActivate?.Invoke();
        }

        private void StartTrading()
        {
            _isTrading = true;
            _tradingTimeLeft = 20;
            _tradeTickTimer = new Timer(1.5f);
            _tradeState = TradeState.WhiteZone;
            _currentTradeValue = 0;
            _tradeView.ShowTradePanel();
        }
        
        private void SetCurrentOrder(ActiveOrder order)
        {
            _currentOrder = order;
            SetTradedState(false);
            _currentMultipler = 1;
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
            int reward = (int)(_currentOrder.Reward * _currentMultipler);
            _tradeView.RewardText.text = reward.ToString();
            _tradeView.Multipler.text = $"+ {(int)((_currentMultipler - 1) * 100)} %";
            TimeSpan timeLeft = TimeSpan.FromSeconds(_tradingTimeLeft);
            _tradeView.Timer.text = timeLeft.ToString("mm' : 'ss");
            _tradeView.TradeSlider.value = _currentTradeValue;
        }
    }
}