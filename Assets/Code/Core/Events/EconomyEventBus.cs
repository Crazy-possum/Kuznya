using System;
using Orders;

namespace GameCoreModule
{
    public class EconomyEventBus
    {
        private Action<UpgradeName, UpgradeView> _onTryBuyUpgrade;
        private Action _onBuyUpgradeCanceled;
        private Action<UpgradeName, UpgradeView, int> _onUpgradeBought;
        private Action<UpgradeName> _onUpgradeApplied;
        private Action<int> _onAddMoney;
        private Action<int> _onRemoveMoney;
        private Action<int> _onMoneyAdded;
        private Action<int> _onMoneyRemoved;
        private Action<int> _onMoneyUpdated;
        private Action<ActiveOrder> _onOrderSubmited;
        private Action<MaterialName, int> _onMaterialRemoved;
        private Action<MaterialConfig> _onCollectableMaterialChanged;
        private Action<MaterialConfig, int> _onMaterialInitialization;
        private Action<MaterialName, int, MaterialName> _onMaterialUpdateInfo;
        private Action _onKingPanelShow;
        

        public Action<UpgradeName, UpgradeView> OnTryBuyUpgrade 
        { get => _onTryBuyUpgrade; set => _onTryBuyUpgrade = value; }
        public Action OnBuyUpgradeCanceled
        { get => _onBuyUpgradeCanceled; set => _onBuyUpgradeCanceled = value; }
        public Action<UpgradeName, UpgradeView, int> OnUpgradeBought 
        { get => _onUpgradeBought; set => _onUpgradeBought = value; }
        public Action<UpgradeName> OnUpgradeApplied
        { get => _onUpgradeApplied; set => _onUpgradeApplied = value; }
        public Action<int> OnAddMoney
        { get => _onAddMoney; set => _onAddMoney = value; }
        public Action<int> OnRemoveMoney
        { get => _onRemoveMoney; set => _onRemoveMoney = value; }
        public Action<int> OnMoneyAdded
        { get => _onMoneyAdded; set => _onMoneyAdded = value; }
        public Action<int> OnMoneyRemoved
        { get => _onMoneyRemoved; set => _onMoneyRemoved = value; }
        public Action<int> OnMoneyUpdated
        { get => _onMoneyUpdated; set => _onMoneyUpdated = value; }
        public Action<ActiveOrder> OnOrderSubmited
        { get => _onOrderSubmited; set => _onOrderSubmited = value; }
        public Action<MaterialName, int> OnMaterialRemoved
        { get => _onMaterialRemoved; set => _onMaterialRemoved = value; }
        public Action<MaterialConfig> OnCollectableMaterialChanged
        { get => _onCollectableMaterialChanged; set => _onCollectableMaterialChanged = value; }
        public Action<MaterialConfig, int> OnMaterialInitialization
        { get => _onMaterialInitialization; set => _onMaterialInitialization = value; }
        public Action<MaterialName, int, MaterialName> OnMaterialUpdateInfo
        { get => _onMaterialUpdateInfo; set => _onMaterialUpdateInfo = value; }

        public Action OnKingPanelShow 
        { get => _onKingPanelShow; set => _onKingPanelShow = value; }
    }
}