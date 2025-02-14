using MAEngine;
using System;

namespace Orders
{
    public class OrdersModule : BasicModule
    {
        public override void Initialise()
        {
            InitializeFields();
            InitializeClientsOperator();
            InitializeOrdersOperator();
            
        }

        private void InitializeFields()
        {
            _actions = new Actions();
        }

        private void InitializeClientsOperator()
        {
            ClientsOperator clientsOperator =
                _di.Resolve<ClientsOperator>();
            _actions.Add(clientsOperator);
        }

        private void InitializeOrdersOperator()
        {
            OrdersOperator ordersOperator =
                _di.Resolve<OrdersOperator>();
            _actions.Add(ordersOperator);
        }

    }
}

