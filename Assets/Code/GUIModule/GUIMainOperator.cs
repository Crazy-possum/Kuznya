using GameCoreModule;
using MAEngine;
using Progression;
using Zenject;

namespace MainGUI
{
    public class GUIMainOperator : IAction, IInitialisation, ICleanUp
    {
        private const string PRE_COST_TEXT = "Желаете потратить";
        private EconomyEventBus _economyEventBus;
        private GUIView _guiView;
        private ProgressionData _progressionData;
        private PlayerMetaData _playerMetaData;
        
        [Inject]
        public void Construct(EconomyEventBus economyEventBus, GUIView guiView, ProgressionData progressionData)
        {
            _economyEventBus = economyEventBus;
            _guiView = guiView;
            _progressionData = progressionData;
        }
        
        public void Initialisation()
        {
            _playerMetaData = _progressionData.PlayerMetaData;
            _economyEventBus.OnMoneyUpdated += UpdateMoneyUI;
            _economyEventBus.OnTryBuyUpgrade += TryBuyUpgrade;
            _guiView.UpgradeConfirmView.gameObject.SetActive(false);
        }

        public void Cleanup()
        {
            _economyEventBus.OnMoneyUpdated -= UpdateMoneyUI;
            _economyEventBus.OnTryBuyUpgrade -= TryBuyUpgrade;
        }
        
        private void UpdateMoneyUI(int money)
        {
            _guiView.CurrentMoneyText.text = $"{money}";
        }
        
        private void TryBuyUpgrade(UpgradeName upgradeName, UpgradeView upgradeView)
        {
            _guiView.UpgradeConfirmView.gameObject.SetActive(true);
            _guiView.UpgradeConfirmView.CostAndInfoText.text =
                $"{PRE_COST_TEXT} {upgradeView.CurrentValueText.text} на :";
            _guiView.UpgradeConfirmView.Description.text = upgradeView.UpgradeConfig.Title;
            _guiView.UpgradeConfirmView.ConfirmButton.onClick.AddListener(
                () => { BuyUpgrade(upgradeName, upgradeView); });
            _guiView.UpgradeConfirmView.CancelButton.onClick.AddListener(CancelUpgrade);
            if (_playerMetaData.CurrentMoney < upgradeView.CurrentValue)
            {
                _guiView.UpgradeConfirmView.ConfirmButton.interactable = false;
            }
            else
            {
                _guiView.UpgradeConfirmView.ConfirmButton.interactable = true;
            }
        }

        private void BuyUpgrade(UpgradeName upgradeName, UpgradeView upgradeView)
        {
            _economyEventBus.OnUpgradeBought?.Invoke(upgradeName, upgradeView, upgradeView.CurrentValue);
            upgradeView.AddLevel();
            _guiView.UpgradeConfirmView.gameObject.SetActive(false);
            _guiView.UpgradeConfirmView.ConfirmButton.onClick.RemoveAllListeners();
            _guiView.UpgradeConfirmView.CancelButton.onClick.RemoveAllListeners();
            _economyEventBus.OnBuyUpgradeCanceled?.Invoke();
        }
        
        private void CancelUpgrade()
        {
            _guiView.UpgradeConfirmView.gameObject.SetActive(false);
            _guiView.UpgradeConfirmView.ConfirmButton.onClick.RemoveAllListeners();
            _guiView.UpgradeConfirmView.CancelButton.onClick.RemoveAllListeners();
            _economyEventBus.OnBuyUpgradeCanceled?.Invoke();
        }
    }
}