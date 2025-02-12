using MAEngine;

namespace Orders
{
    public class OrdersModule : BasicModule
    {
        public override void Initialise()
        {
            InitializeFields();
            InitializeOrdersActions();
        }

        private void InitializeFields()
        {
            _actions = new Actions();
        }

        private void InitializeOrdersActions()
        {

        }
    }
}

