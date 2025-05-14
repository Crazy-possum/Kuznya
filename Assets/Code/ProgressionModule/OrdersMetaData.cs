using System;
using Orders;
using System.Collections.Generic;
using GameCoreModule;
using MAEngine.Extention;
using UnityEngine;
using UnityEngine.Serialization;

namespace Progression
{
    [Serializable]
    public class OrdersMetaData
    {
        [SerializeField] private List<ClientConfig> _activeClients;
        [SerializeField] private List<ActiveOrder> _activeOrders;
        [SerializeField] private ClientConfig _activeClient;
        [FormerlySerializedAs("_whoresaleOrder")] [SerializeField] private ActiveOrder _wholesaleOrder;
        [SerializeField] private float _wholesaleOrderTime;
        
        public List<ClientConfig> ActiveClients { get => _activeClients; set => _activeClients = value; }
        public List<ActiveOrder> ActiveOrders { get => _activeOrders; set => _activeOrders = value; }
        public ClientConfig ActiveClient { get => _activeClient; set => _activeClient = value; }
        public ActiveOrder WholesaleOrder { get => _wholesaleOrder; set => _wholesaleOrder = value; }
        public float WholesaleOrderTime {get => _wholesaleOrderTime; set => _wholesaleOrderTime = value; }
        

        public void Initialize(SaveLoadEventBus saveLoadEventBus, ClientsPoolConfig clientsPoolConfig)
        {
            LoadData(saveLoadEventBus, clientsPoolConfig);
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

        public void Clear()
        {
            _activeClients.Clear();
            _activeOrders.Clear();
            _activeClient = null;
        }

        public ActiveOrder GetActiveOrder(int index)
        {
            ActiveOrder order = _activeOrders[index];
            order.PreLoadOrder();
            return order;
        }
        
        public void SaveActiveOrder(ActiveOrder order)
        {
            order.PreSaveOrder();
            if (!_activeOrders.Contains(order))
            {
                _activeOrders.Add(order);
            }
        }

        public void SaveData(SaveLoadEventBus saveLoadEventBus)
        {
            string activeClientsString = "";
            foreach (ClientConfig client in _activeClients)
            {
                activeClientsString += $"],[{client.ToString()}";
            }
            saveLoadEventBus.OnSaveString?.Invoke(SaveDataKey.ActiveClients, activeClientsString);
            string activeOrdersString = "";
            foreach (ActiveOrder order in _activeOrders)
            {
                activeOrdersString += $"],[{order.ToString()}";
            }
            saveLoadEventBus.OnSaveString?.Invoke(SaveDataKey.ActiveOrders, activeOrdersString);
            if (_activeClient != null)
            {
                saveLoadEventBus.OnSaveString?.Invoke(SaveDataKey.ActiveClient, _activeClient.ToString());
            }
            string whoresaleOrderString = "";
            if (_wholesaleOrder != null && _wholesaleOrder.OrderTimer != null)
            {
                whoresaleOrderString = _wholesaleOrder.ToString();
            }
            saveLoadEventBus.OnSaveString?.Invoke(SaveDataKey.WhoresaleOrder, whoresaleOrderString);
            saveLoadEventBus.OnSaveFloat?.Invoke(SaveDataKey.WhoresaleTime, _wholesaleOrderTime);
            
        }

        public void LoadData(SaveLoadEventBus saveLoadEventBus, ClientsPoolConfig clientsPoolConfig)
        {
            SavableString savableString = new SavableString();
            saveLoadEventBus.OnGetString?.Invoke(SaveDataKey.ActiveClients, savableString);
            if (savableString.IsLoaded)
            {
                string activeClientsString = savableString.Value;
                SetupActiveClients(activeClientsString, clientsPoolConfig);
            }
            else
            {
                _activeClients = new List<ClientConfig>();
            }
            savableString = new SavableString();
            saveLoadEventBus.OnGetString?.Invoke(SaveDataKey.ActiveOrders, savableString);
            if (savableString.IsLoaded)
            {
                string activeOrdersString = savableString.Value;
                SetupActiveOrders(activeOrdersString, clientsPoolConfig);
            }
            else
            {
                _activeOrders = new List<ActiveOrder>();
            }
            savableString = new SavableString();
            saveLoadEventBus.OnGetString?.Invoke(SaveDataKey.ActiveClient, savableString);
            if (savableString.IsLoaded)
            {
                string activeClientString = savableString.Value;
                SetupActiveClient(activeClientString, clientsPoolConfig);
            }
            else
            {
                _activeClient = null;
            }
            savableString = new SavableString();
            saveLoadEventBus.OnGetString?.Invoke(SaveDataKey.WhoresaleOrder, savableString);
            if (savableString.IsLoaded)
            {
                string whoresaleOrderString = savableString.Value;
                _wholesaleOrder = new ActiveOrder(clientsPoolConfig);
                _wholesaleOrder.LoadOrder(whoresaleOrderString);
            }
            else
            {
                _wholesaleOrder = null;
            }
            SavableFloat savableFloat = new SavableFloat();
            saveLoadEventBus.OnGetFloat?.Invoke(SaveDataKey.WhoresaleTime, savableFloat);
            if (savableFloat.IsLoaded)
            {
                _wholesaleOrderTime = savableFloat.Value;
            }
            else
            {
                _wholesaleOrderTime = 0;
            }
        }
        
        private void SetupActiveClients(string activeClientsString, ClientsPoolConfig clientsPoolConfig)
        {
            string[] activeClientsArray =
                activeClientsString.Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries);
            _activeClients = new List<ClientConfig>();
            foreach (string clientString in activeClientsArray)
            {
                foreach (ClientConfig clientConfig in clientsPoolConfig.Clients)
                {
                    if (clientConfig.name == clientString)
                    {
                        _activeClients.Add(clientConfig);
                        break;
                    }
                }
            }
        }
        
        private void SetupActiveOrders(string activeOrdersString, ClientsPoolConfig clientsPoolConfig)
        {
            string[] activeOrdersArray =
                activeOrdersString.Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries);
            _activeOrders = new List<ActiveOrder>();
            foreach (string orderString in activeOrdersArray)
            {
                ActiveOrder order = new ActiveOrder(clientsPoolConfig);
                order.LoadOrder(orderString);
                _activeOrders.Add(order);
            }
        }
        
        private void SetupActiveClient(string activeClientString, ClientsPoolConfig clientsPoolConfig)
        {
            foreach (ClientConfig clientConfig in clientsPoolConfig.Clients)
            {
                if (clientConfig.name == activeClientString)
                {
                    _activeClient = clientConfig;
                    break;
                }
            }
        }

        public void SaveWholesaleOrder(ActiveOrder whoresaleActiveOrder)
        {
            //Debug.Log("WholesaleSaving");
            _wholesaleOrder = whoresaleActiveOrder;
        }
    }
}

