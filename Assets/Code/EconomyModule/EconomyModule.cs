using MAEngine;

namespace Economy
{
    public class EconomyModule : BasicModule
    {
        public override void Initialise()
        {
            InitializeFields();
            InitializeShopUIActions();
            InitializeEconomyOperator();
            InitializeTradeController();
        }

        private void InitializeFields()
        {
            _actions = new Actions();
        }

        private void InitializeShopUIActions()
        {
            UpgradesUIActions upgradesUIActions =
                _di.Resolve<UpgradesUIActions>();
            _actions.Add(upgradesUIActions);
        }
        
        private void InitializeEconomyOperator()
        {
            EconomyOperator economyOperator =
                _di.Resolve<EconomyOperator>();
            _actions.Add(economyOperator);
        }
        
        private void InitializeTradeController()
        {
            TradeController tradeController =
                _di.Resolve<TradeController>();
            _actions.Add(tradeController);
        }
    }
}

