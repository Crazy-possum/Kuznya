using System;
using Orders;

namespace GameCoreModule
{
    public class EconomyEventBus
    {
        private Action<UpgradeName, UpgradeView> _onTryBuyUpgrade;
        private Action _onBuyUpgradeCanceled;
        private Action<UpgradeName, UpgradeView, int> _onUpgradeBought;
        private Action<int> _onAddMoney;
        private Action<int> _onMoneyAdded;
        private Action<int> _onMoneyRemoved;
        private Action<int> _onMoneyUpdated;
        private Action<ActiveOrder> _onOrderSubmited;
        private Action<MaterialName, int> _onMaterialRemoved;
        private Action<MaterialConfig> _onCollectableMaterialChanged;
        private Action<MaterialConfig, int> _onMaterialInitialization;
        private Action<MaterialName, int, MaterialName> _onMaterialUpdateInfo;
        

        public Action<UpgradeName, UpgradeView> OnTryBuyUpgrade 
        { get => _onTryBuyUpgrade; set => _onTryBuyUpgrade = value; }
        public Action OnBuyUpgradeCanceled
        { get => _onBuyUpgradeCanceled; set => _onBuyUpgradeCanceled = value; }
        public Action<UpgradeName, UpgradeView, int> OnUpgradeBought 
        { get => _onUpgradeBought; set => _onUpgradeBought = value; }
        public Action<int> OnAddMoney
        { get => _onAddMoney; set => _onAddMoney = value; }
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
    }
}