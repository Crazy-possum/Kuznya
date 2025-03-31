using Orders;
using System;

namespace GameCoreModule
{
    public class OrdersEventBus
    {
        private Action _onClientActivated;
        private Action _onClientDeactivated;
        private Action<ClientConfig> _onClientAdded;
        private Action _onClientRemoved;
        private Action _onOrderAdded;
        private Action _onOrderRemoved;
        private Action<ActiveOrder> _onOrderStarted;
        private Action<ActiveOrder> _onOrderFinished;
        private Action<ActiveOrder> _onOrderEnded;

        public Action OnClientActivated { get => _onClientActivated; set => _onClientActivated = value; }
        public Action OnClientDeactivated { get => _onClientDeactivated; set => _onClientDeactivated = value; }
        public Action<ClientConfig> OnClientAdded { get => _onClientAdded; set => _onClientAdded = value; }
        public Action OnClientRemoved { get => _onClientRemoved; set => _onClientRemoved = value; }
        public Action OnOrderAdded { get => _onOrderAdded; set => _onOrderAdded = value; }
        public Action OnOrderRemoved { get => _onOrderRemoved; set => _onOrderRemoved = value; }
        public Action<ActiveOrder> OnOrderStarted { get => _onOrderStarted; set => _onOrderStarted = value; }
        public Action<ActiveOrder> OnOrderFinished { get => _onOrderFinished; set => _onOrderFinished = value; }
        public Action<ActiveOrder> OnOrderEnded { get => _onOrderEnded; set => _onOrderEnded = value; }
    }
}
