using GameCoreModule;
using MAEngine;
using Orders;
using Progression;
using UnityEngine;
using Zenject;

namespace MainGUI
{
    public class GUIMainOperator : IAction, IInitialisation, ICleanUp, IFixedExecute
    {
        private const string PRE_COST_TEXT = "Желаете потратить";
        private EconomyEventBus _economyEventBus;
        private GUIView _guiView;
        private ProgressionData _progressionData;
        private PlayerMetaData _playerMetaData;
        private ProgressionEvents _progressionEvents;
        private OrdersEventBus _ordersEventBus;
        private StateEventsBus _stateEventsBus;
        private UIEventBus _uiEventBus;
        
        private ActiveOrder _activeOrder;
        
        [Inject]
        public void Construct(EconomyEventBus economyEventBus, GUIView guiView, ProgressionData progressionData,
            ProgressionEvents progressionEvents, OrdersEventBus ordersEventBus, StateEventsBus stateEventsBus,
            UIEventBus uiEventBus)
        {
            _economyEventBus = economyEventBus;
            _guiView = guiView;
            _progressionData = progressionData;
            _progressionEvents = progressionEvents;
            _ordersEventBus = ordersEventBus;
            _stateEventsBus = stateEventsBus;
            _uiEventBus = uiEventBus;
        }
        
        public void Initialisation()
        {
            Screen.SetResolution(1920, 1080, true);
            _playerMetaData = _progressionData.PlayerMetaData;
            _economyEventBus.OnMoneyUpdated += UpdateMoneyUI;
            _economyEventBus.OnTryBuyUpgrade += TryBuyUpgrade;
            _ordersEventBus.OnOrderEnded += ActiveOrderEnded;
            _ordersEventBus.OnOrderStarted += SetOrderGUI;
            _ordersEventBus.OnOrderFinished += HideOrderGUI;
            _guiView.UpgradeConfirmView.gameObject.SetActive(false);
            _guiView.ClearProgressButton.onClick.AddListener(ClearProgress);
            _uiEventBus._onFreezeUI += FreezeUI;
            _uiEventBus._onUnfreezeUI += UnfreezeUI;
        }

        private void ClearProgress()
        {
            _progressionEvents.OnProgressionDataCleared?.Invoke();
        }

        public void Cleanup()
        {
            _economyEventBus.OnMoneyUpdated -= UpdateMoneyUI;
            _economyEventBus.OnTryBuyUpgrade -= TryBuyUpgrade;
            _ordersEventBus.OnOrderEnded -= ActiveOrderEnded;
            _ordersEventBus.OnOrderStarted -= SetOrderGUI;
            _ordersEventBus.OnOrderFinished -= HideOrderGUI;
            _guiView.ClearProgressButton.onClick.RemoveListener(ClearProgress);
            _uiEventBus._onFreezeUI -= FreezeUI;
            _uiEventBus._onUnfreezeUI -= UnfreezeUI;
        }
        
        public void FixedExecute(float fixedDeltaTime)
        {
            if (_activeOrder != null)
            {
                float delta = _activeOrder.OrderTime - _activeOrder.OrderTimer.GetRemainingTime();
                _guiView.OrderTimeSlider.value =  _activeOrder.OrderTime - delta;
            }
        }
        
        private void ActiveOrderEnded(ActiveOrder order)
        {
            _guiView.OrderEndedPanel.SetActive(true);
            _stateEventsBus.OnDialogueStateActivate?.Invoke();
            HideOrderGUI(order);
        }
        
        private void SetOrderGUI(ActiveOrder order)
        {
            _guiView.ClientsQueueTransform.gameObject.SetActive(false);
            _guiView.OrderTimePanel.SetActive(true);
            _guiView.OrderTimeSlider.maxValue = order.OrderTime;
            float delta = order.OrderTime - order.OrderTimer.GetRemainingTime();
            _guiView.OrderTimeSlider.value = order.OrderTime - delta;
            _activeOrder = order;
        }
        
        private void HideOrderGUI(ActiveOrder order)
        {
            _guiView.ClientsQueueTransform.gameObject.SetActive(true);
            _guiView.OrderTimePanel.SetActive(false);
            _guiView.NavigationPanel.gameObject.SetActive(true);
            _activeOrder = null;

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
        
        private void FreezeUI()
        {
            _guiView.UIBlockingPanel.SetActive(true);
        }

        private void UnfreezeUI()
        {
            _guiView.UIBlockingPanel.SetActive(false);
        }
    }
}