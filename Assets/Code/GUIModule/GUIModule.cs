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
        }

        private void InitializeFields()
        {
            _actions = new Actions();
        }

        private void InitializeGUIButtonsOperator()
        {
            GUIButtonsOperator gUIButtonsOperator =
                _di.Resolve<GUIButtonsOperator>();
            _actions.Add(gUIButtonsOperator);
        }
    }
}

