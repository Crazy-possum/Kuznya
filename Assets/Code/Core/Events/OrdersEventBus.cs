using System;

namespace GameCoreModule
{
    public class OrdersEventBus
    {
        private Action _onClientActivated;
        private Action _onClientDeactivated;
        private Action _onClientAdded;
        private Action _onClientRemoved;
        private Action _onOrderAdded;

        public Action OnClientActivated { get => _onClientActivated; set => _onClientActivated = value; }
        public Action OnClientDeactivated { get => _onClientDeactivated; set => _onClientDeactivated = value; }
        public Action OnClientAdded { get => _onClientAdded; set => _onClientAdded = value; }
        public Action OnClientRemoved { get => _onClientRemoved; set => _onClientRemoved = value; }
        public Action OnOrderAdded { get => _onOrderAdded; set => _onOrderAdded = value; }

    }
}
