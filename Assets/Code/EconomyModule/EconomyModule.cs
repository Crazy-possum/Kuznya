using System.Collections;
using System.Collections.Generic;
using MAEngine;

namespace Economy
{
    public class EconomyModule : BasicModule
    {
        public override void Initialise()
        {
            InitializeFields();
            InitializeShopUIActions();
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
    }

    public class EconomyOperator
    {
        
    }
}

