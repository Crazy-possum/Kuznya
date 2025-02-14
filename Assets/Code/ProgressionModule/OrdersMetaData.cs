using Orders;
using System.Collections.Generic;

namespace Progression
{
    public class OrdersMetaData
    {
        private List<ClientConfig> _activeClients;
        private List<ActiveOrder> _activeOrders;

        public List<ActiveOrder> ActiveOrders { get => _activeOrders; set => _activeOrders = value; }

        public void Initialize()
        {
            _activeClients = new List<ClientConfig>();
            _activeOrders = new List<ActiveOrder>();
        }

        public void Initialize(List<ClientConfig> activeClients,
            List<ActiveOrder> activeOrders)
        {
            _activeClients = activeClients;
            _activeOrders = activeOrders;
        }

        public void AddClient(ClientConfig client)
        {
            _activeClients.Add(client);
        }

        public ClientConfig GetFirstClient()
        {
            if (_activeClients.Count == 0)
            {
                return null;
            }
            ClientConfig client = _activeClients[0];
            _activeClients.Remove(client);
            return client;
        }

        public ClientConfig GetFirstClientWithoutRemoving()
        {
            if (_activeClients.Count == 0)
            {
                return null;
            }
            ClientConfig client = _activeClients[0];
            _activeClients.Remove(client);
            return client;
        }

        public int GetClientsCount()
        {
            return _activeClients.Count;
        }

        public bool CheckClientIsFree(ClientConfig client, ClientConfig activeClient)
        {
            bool isClientFree = true;
            isClientFree = !_activeClients.Contains(client) && client != activeClient;
            return isClientFree;
        }

    }
}

