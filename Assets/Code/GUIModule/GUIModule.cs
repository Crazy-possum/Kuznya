using MAEngine;
using System;

namespace MainGUI
{
    public class GUIModule : BasicModule
    {
        public override void Initialise()
        {
            InitializeFields();
            InitializeGUIButtonsOperator();
            InitializeShopScaler();
        }

        private void InitializeFields()
        {
            _actions = new Actions();
        }

        private void InitializeGUIButtonsOperator()
        {
            GUINavigationButtonsOperator gUINavigationButtonsOperator =
                _di.Resolve<GUINavigationButtonsOperator>();
            _actions.Add(gUINavigationButtonsOperator);
        }
        
        private void InitializeShopScaler()
        {
            UIScalingAction uiScalingAction =
                _di.Resolve<UIScalingAction>();
            _actions.Add(uiScalingAction);
        }
    }
}

