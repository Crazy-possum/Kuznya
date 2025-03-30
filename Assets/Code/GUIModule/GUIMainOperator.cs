using GameCoreModule;
using MAEngine;
using Progression;
using UnityEngine;
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
        private ProgressionEvents _progressionEvents;
        
        [Inject]
        public void Construct(EconomyEventBus economyEventBus, GUIView guiView, ProgressionData progressionData,
            ProgressionEvents progressionEvents)
        {
            _economyEventBus = economyEventBus;
            _guiView = guiView;
            _progressionData = progressionData;
            _progressionEvents = progressionEvents;
        }
        
        public void Initialisation()
        {
            Screen.SetResolution(1920, 1080, true);
            _playerMetaData = _progressionData.PlayerMetaData;
            _economyEventBus.OnMoneyUpdated += UpdateMoneyUI;
            _economyEventBus.OnTryBuyUpgrade += TryBuyUpgrade;
            _guiView.UpgradeConfirmView.gameObject.SetActive(false);
            _guiView.ClearProgressButton.onClick.AddListener(ClearProgress);
        }

        private void ClearProgress()
        {
            _progressionEvents.OnProgressionDataCleared?.Invoke();
        }

        public void Cleanup()
        {
            _economyEventBus.OnMoneyUpdated -= UpdateMoneyUI;
            _economyEventBus.OnTryBuyUpgrade -= TryBuyUpgrade;
            _guiView.ClearProgressButton.onClick.RemoveListener(ClearProgress);
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
            int upgradePrice = upgradeView.CurrentValue;
            upgradeView.AddLevel();
            _economyEventBus.OnUpgradeBought?.Invoke(upgradeName, upgradeView, upgradePrice);
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