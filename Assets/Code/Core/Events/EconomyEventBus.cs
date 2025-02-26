using System;

namespace GameCoreModule
{
    public class EconomyEventBus
    {
        private Action<UpgradeName, UpgradeView> _onTryBuyUpgrade;
        private Action _onBuyUpgradeCanceled;
        private Action<UpgradeName, UpgradeView, int> _onUpgradeBought;
        private Action<int> _onMoneyAdded;
        private Action<int> _onMoneyRemoved;
        private Action<int> _onMoneyUpdated;

        public Action<UpgradeName, UpgradeView> OnTryBuyUpgrade 
        { get => _onTryBuyUpgrade; set => _onTryBuyUpgrade = value; }
        public Action OnBuyUpgradeCanceled
        { get => _onBuyUpgradeCanceled; set => _onBuyUpgradeCanceled = value; }
        public Action<UpgradeName, UpgradeView, int> OnUpgradeBought 
        { get => _onUpgradeBought; set => _onUpgradeBought = value; }
        public Action<int> OnMoneyAdded
        { get => _onMoneyAdded; set => _onMoneyAdded = value; }
        public Action<int> OnMoneyRemoved
        { get => _onMoneyRemoved; set => _onMoneyRemoved = value; }
        public Action<int> OnMoneyUpdated
        { get => _onMoneyUpdated; set => _onMoneyUpdated = value; }
    }
}