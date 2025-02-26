using System;

namespace GameCoreModule
{
    public class EconomyEventBus
    {
        private Action<UpgradeName, UpgradeView> _onTryBuyUpgrade;
        private Action<UpgradeName, UpgradeView> _onUpgradeBought;
        private Action<int> _onMoneyAdded;
        private Action<int> _onMoneyRemoved;

        public Action<UpgradeName, UpgradeView> OnTryBuyUpgrade 
        { get => _onTryBuyUpgrade; set => _onTryBuyUpgrade = value; }
        public Action<UpgradeName, UpgradeView> OnUpgradeBought 
        { get => _onUpgradeBought; set => _onUpgradeBought = value; }
        public Action<int> OnMoneyAdded
        { get => _onMoneyAdded; set => _onMoneyAdded = value; }
        public Action<int> OnMoneyRemoved
        { get => _onMoneyRemoved; set => _onMoneyRemoved = value; }
    }
}