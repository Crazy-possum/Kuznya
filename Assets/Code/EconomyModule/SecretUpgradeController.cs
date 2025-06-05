using GameCoreModule;
using MAEngine;
using Progression;
using UnityEngine;
using Zenject;

namespace Economy
{
    public class SecretUpgradeController : IAction, IInitialisation, ICleanUp
    {
        private const int GIVE_MONEY_AMOUNT = 5000;
        
        private EconomyEventBus _economyEventBus;
        private ProgressionData _progressionData;
        private KingPanelView _kingPanelView;
        private ResultsEventBus _resultsEventBus;

        private bool _isKingPanelSubscribed;

        [Inject]
        public void Construct(EconomyEventBus economyEventBus, ProgressionData progressionData,
            KingPanelView kingPanelView, ResultsEventBus resultsEventBus)
        {
            _economyEventBus = economyEventBus;
            _progressionData = progressionData;
            _kingPanelView = kingPanelView;
            _resultsEventBus = resultsEventBus;
        }

        public void Initialisation()
        {
            _economyEventBus.OnUpgradeApplied += CheckSecretUpgradeApplied;
            if (_progressionData.UpgradesMeta.Upgrades.IsContainsKey(UpgradeName.SecretUpgrade))
            {
                _economyEventBus.OnKingPanelShow += ActiveteKingPanel;
                _resultsEventBus.OnResultsFinished += ActiveteKingPanel;
            }
        }

        public void Cleanup()
        {
            _economyEventBus.OnUpgradeApplied -= CheckSecretUpgradeApplied;
        }

        private void CheckSecretUpgradeApplied(UpgradeName upgradeName)
        {
            if (upgradeName == UpgradeName.SecretUpgrade)
            {
                _resultsEventBus.OnResultsFinished += ActiveteKingPanel;
                ActiveteKingPanel();
            }
        }

        private void ActiveteKingPanel(int money)
        {
            ActiveteKingPanel();
        }
        
        private void ActiveteKingPanel()
        {
            _kingPanelView.KingPanel.SetActive(true);
            if (!_isKingPanelSubscribed)
            {
                _isKingPanelSubscribed = true;
                _kingPanelView.GiveMoneyButton.onClick.AddListener(GiveMoneyToKing);
            }

            if (_progressionData.PlayerMetaData.CurrentMoney >= GIVE_MONEY_AMOUNT)
            {
                _kingPanelView.GiveMoneyButton.interactable = true;
            }
            else
            {
                _kingPanelView.GiveMoneyButton.interactable = false;
            }
        }

        private void GiveMoneyToKing()
        {
            Debug.Log(_economyEventBus);
            _economyEventBus.OnRemoveMoney?.Invoke(GIVE_MONEY_AMOUNT);
            _kingPanelView.KingPanel.SetActive(false);
        }
        
    }
}