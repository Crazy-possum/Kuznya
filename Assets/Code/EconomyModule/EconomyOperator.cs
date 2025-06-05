using GameCoreModule;
using MAEngine;
using Orders;
using Progression;
using UnityEngine;
using Zenject;

namespace Economy
{
    public class EconomyOperator : IAction, IInitialisation, ICleanUp
    {
        private EconomyEventBus _economyEventBus;
        private ProgressionData _progressionData;
        private OrdersEventBus _ordersEventBus;
        private StateEventsBus _stateEventsBus;
        private ShopView _shopView;
        private TutorialEventBus _tutorialEventBus;
        
        private PlayerMetaData _playerMetaData;
        
        [Inject]
        public void Construct(EconomyEventBus economyEventBus,
            OrdersEventBus ordersEventBus, ProgressionData progressionData,
            StateEventsBus stateEventsBus, ShopView shopView,
            TutorialEventBus tutorialEventBus)
        {
            _economyEventBus = economyEventBus;
            _ordersEventBus = ordersEventBus;
            _progressionData = progressionData;
            _stateEventsBus = stateEventsBus;
            _shopView = shopView;
            _tutorialEventBus = tutorialEventBus;
        }
        
        public void Initialisation()
        {
            _playerMetaData = _progressionData.PlayerMetaData;
            _ordersEventBus.OnOrderFinished += StartTrade;
            _economyEventBus.OnAddMoney += AddMoney;
            _economyEventBus.OnUpgradeBought += RemoveMoney;
            _economyEventBus.OnRemoveMoney += RemoveMoney;
            _economyEventBus.OnMoneyUpdated += UpdateButtonsState;
            _economyEventBus.OnMoneyUpdated?.Invoke(_playerMetaData.CurrentMoney);
        }

        private void StartTrade(ActiveOrder order)
        {
            _economyEventBus.OnOrderSubmited?.Invoke(order);
            _stateEventsBus.OnTradeStateActivate?.Invoke();
        }

        private void UpdateButtonsState(int currentMoney)
        {
            foreach (UpgradeView upgradeView in _shopView.UpgradeViews.GetAllValues())
            {
                if (upgradeView.CurrentValue <= currentMoney)
                {
                    if (upgradeView.CurrentLevel < upgradeView.UpgradeConfig.LevelsCostList.Count)
                    {
                        upgradeView.UpgradeButton.interactable = true;
                    }
                }
                else
                {
                    upgradeView.UpgradeButton.interactable = false;
                }
            }
        }

        public void Cleanup()
        {
            _ordersEventBus.OnOrderFinished -= StartTrade;
            _economyEventBus.OnAddMoney -= AddMoney;
            _economyEventBus.OnUpgradeBought -= RemoveMoney;
            _economyEventBus.OnMoneyUpdated -= UpdateButtonsState;
        }
        
        private void AddMoney(int money)
        {
            _economyEventBus.OnMoneyAdded?.Invoke(money);
            _playerMetaData.CurrentMoney += money;
            _economyEventBus.OnMoneyUpdated?.Invoke(_playerMetaData.CurrentMoney);
            _tutorialEventBus.OnMoneyAdded?.Invoke();
            if (_playerMetaData.CurrentMoney >= 1500)
            {
                _tutorialEventBus.OnMoneyAccumulated?.Invoke();
            }
        }
        
        private void RemoveMoney(int money)
        {
            _economyEventBus.OnMoneyRemoved?.Invoke(money);
            _playerMetaData.CurrentMoney -= money;
        }
        
        private void RemoveMoney(UpgradeName upgradeName, UpgradeView upgradeView, int money)
        {
            _economyEventBus.OnMoneyRemoved?.Invoke(money);
            _playerMetaData.CurrentMoney -= money;
            _economyEventBus.OnMoneyUpdated?.Invoke(_playerMetaData.CurrentMoney);
        }
    }
}