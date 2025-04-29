using MAEngine.Extention;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Orders
{
    [Serializable]
    public class ActiveOrder
    {
        private string _clientName;
        private Sprite _clientIcon;
        private string _name;
        private OrderText _description;
        private Sprite _orderIcon;
        private List<ForgingMaterial> _materials;
        private int _basicCost;
        private float _orderTime;
        private Timer _orderTimer;
        private bool _isCompleted;
        private int _orderCount;
        private int _currentOrderCount;
        private int _reward;
        private OrderType _orderType;
        private float _remainingTime;
        private string _orderID;
        private string _clientID;
        private int _descriptionID;
        private ClientsPoolConfig _clientsPoolConfig;

        public string ClientName { get => _clientName; }
        public Sprite ClientIcon { get => _clientIcon; }
        public string Name { get => _name; }
        public OrderText Description { get => _description; }
        public Sprite OrderIcon { get => _orderIcon; }
        public List<ForgingMaterial> Materials { get => _materials; }
        public int BasicCost { get => _basicCost; set => _basicCost = value; }
        public float OrderTime { get => _orderTime; }
        public Timer OrderTimer { get => _orderTimer; set => _orderTimer = value; }
        public bool IsCompleted { get => _isCompleted; set => _isCompleted = value; }
        public int OrderCount { get => _orderCount; set => _orderCount = value; }
        public int CurrentOrderCount { get => _currentOrderCount; set => _currentOrderCount = value; }
        public int Reward { get => _reward; set => _reward = value; }
        public OrderType OrderType { get => _orderType; }
        public float RemainingTime { get => _remainingTime; }
        public string OrderID { get => _orderID; set => _orderID = value; }
        public string ClientID { get => _clientID; set => _clientID = value; }
        public int DecriptionID { get => _descriptionID; set => _descriptionID = value; }
        
        public ActiveOrder(ClientsPoolConfig clientsPoolConfig)
        {
            _clientsPoolConfig = clientsPoolConfig;
        }

        public ActiveOrder(string name, OrderText description,
            Sprite orderIcon, List<ForgingMaterial> materials,
            int orderTime, bool isCompleted = false)
        {
            _name = name;
            _description = description;
            _orderIcon = orderIcon;
            _materials = materials;
            _orderTimer = new Timer(orderTime);
            _isCompleted = isCompleted;
        }

        public ActiveOrder(ClientConfig client, OrderConfig orderConfig, int DecriptionID)
        {
            _clientName = client.Name;
            _clientIcon = client.ClientSprite;
            _name = orderConfig.Name;
            _description = orderConfig.Descriptions[DecriptionID];
            _orderIcon = orderConfig.OrderIcon;
            _materials = orderConfig.Materials;
            _orderTime = orderConfig.OrderTime;
            _basicCost = orderConfig.BasicCost;
            _orderType = orderConfig.OrderType;
            _orderCount = orderConfig.OrderCount;
            _currentOrderCount = 0;
            _orderTimer = new Timer(orderConfig.OrderTime);
            _isCompleted = false;
            _orderID = orderConfig.name;
            _clientID = client.name;
            _descriptionID = DecriptionID;
        }

        public void PreSaveOrder()
        {
            _remainingTime = _orderTimer.GetRemainingTime();
        }
        
        public void PreLoadOrder()
        {
            _orderTimer = new Timer(_orderTime);
            float timeDelta = _orderTimer.GetRemainingTime() - _remainingTime;
            _orderTimer.StartWaitTime = Time.time - timeDelta;
        }

        public override string ToString()
        {
            return $"{_orderID}|_|{_clientID}|_|{_orderTimer.GetRemainingTime()}|_|{_descriptionID}";
        }

        public void LoadOrder(string savedData)
        {
            string[] items = savedData.Split(new string[] { "|_|" }, StringSplitOptions.RemoveEmptyEntries);
            if (items.Length < 4)
            {
                Debug.LogError("Invalid saved data format");
                return;
            }
            else
            {
                _orderID = items[0];
                _clientID = items[1];
                _descriptionID = int.Parse(items[3]);
                ClientConfig client = SetupClientData(_clientID);
                SetupOrderData(client, _orderID, _descriptionID);
                _remainingTime = float.Parse(items[2]);
                PreLoadOrder();
            }
        }

        private void SetupOrderData(ClientConfig client, string orderID, int descriptionID)
        {
            foreach (OrderConfig order in client.Orders.GetOrders())
            {
                if (order.name == orderID)
                {
                    _name = order.Name;
                    _description = order.Descriptions[descriptionID];
                    _orderIcon = order.OrderIcon;
                    _materials = order.Materials;
                    _orderTime = order.OrderTime;
                    _basicCost = order.BasicCost;
                    _orderType = order.OrderType;
                    _isCompleted = false;
                }
            }
        }

        private ClientConfig SetupClientData(string clientID)
        {
            foreach (ClientConfig client in _clientsPoolConfig.Clients)
            {
                if (client.name == clientID)
                {
                    _clientName = client.Name;
                    _clientIcon = client.ClientSprite;
                    return client;
                }
            }
            Debug.LogError($"Client with ID {clientID} not found");
            return null;
        }
        
        public bool CheckIsOrderCountCompleted()
        {
            _currentOrderCount++;
            if (_currentOrderCount >= _orderCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

