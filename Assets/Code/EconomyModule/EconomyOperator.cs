using GameCoreModule;
using MAEngine;
using Progression;
using UnityEngine;
using Zenject;

namespace Economy
{
    public class EconomyOperator : IAction, IInitialisation, ICleanUp
    {
        private ResultsEventBus _resultsEventBus;
        private EconomyEventBus _economyEventBus;
        private ProgressionData _progressionData;
        private ShopView _shopView;
        
        private PlayerMetaData _playerMetaData;
        
        [Inject]
        public void Construct(ResultsEventBus resultsEventBus, EconomyEventBus economyEventBus,
            ProgressionData progressionData, ShopView shopView)
        {
            _resultsEventBus = resultsEventBus;
            _economyEventBus = economyEventBus;
            _progressionData = progressionData;
            _shopView = shopView;
        }
        
        public void Initialisation()
        {
            _playerMetaData = _progressionData.PlayerMetaData;
            _resultsEventBus.OnResultsFinished += AddMoney;
            _economyEventBus.OnUpgradeBought += RemoveMoney;
            _economyEventBus.OnMoneyUpdated += UpdateButtonsState;
            _economyEventBus.OnMoneyUpdated?.Invoke(_playerMetaData.CurrentMoney);
        }

        private void UpdateButtonsState(int currentMoney)
        {
            foreach (UpgradeView upgradeView in _shopView.UpgradeViews.GetAllValues())
            {
                if (upgradeView.CurrentValue <= currentMoney)
                {
                    upgradeView.UpgradeButton.interactable = true;
                }
                else
                {
                    upgradeView.UpgradeButton.interactable = false;
                }
            }
        }

        public void Cleanup()
        {
            _resultsEventBus.OnResultsFinished -= AddMoney;
            _economyEventBus.OnUpgradeBought -= RemoveMoney;
        }
        
        private void AddMoney(int money)
        {
            _economyEventBus.OnMoneyAdded?.Invoke(money);
            _playerMetaData.CurrentMoney += money;
            _economyEventBus.OnMoneyUpdated?.Invoke(_playerMetaData.CurrentMoney);
        }
        
        private void RemoveMoney(UpgradeName upgradeName, UpgradeView upgradeView, int money)
        {
            _economyEventBus.OnMoneyRemoved?.Invoke(money);
            _playerMetaData.CurrentMoney -= money;
            _economyEventBus.OnMoneyUpdated?.Invoke(_playerMetaData.CurrentMoney);
        }
    }
}