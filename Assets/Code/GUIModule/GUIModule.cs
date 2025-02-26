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
            InitializeGUIMainOperator();
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
        
        private void InitializeGUIMainOperator()
        {
            GUIMainOperator guiMainOperator =
                _di.Resolve<GUIMainOperator>();
            _actions.Add(guiMainOperator);
        }
    }
}

