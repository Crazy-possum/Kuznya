using MAEngine;

namespace MainGUI
{
    public class GUIModule : BasicModule
    {
        public override void Initialise()
        {
            InitializeFields();
        }


        private void InitializeFields()
        {
            _actions = new Actions();
        }
    }
}

