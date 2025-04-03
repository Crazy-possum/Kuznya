using MAEngine.Extention;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        private int _reward;
        private OrderType _orderType;
        private float _remainingTime;

        public string ClientName { get => _clientName; }
        public Sprite ClientIcon { get => _clientIcon; }
        public string Name { get => _name; }
        public OrderText Description { get => _description; }
        public Sprite OrderIcon { get => _orderIcon; }
        public List<ForgingMaterial> Materials { get => _materials; }
        public int BasicCost { get => _basicCost; }
        public float OrderTime { get => _orderTime; }
        public Timer OrderTimer { get => _orderTimer; set => _orderTimer = value; }
        public bool IsCompleted { get => _isCompleted; set => _isCompleted = value; }
        public int Reward { get => _reward; set => _reward = value; }
        public OrderType OrderType { get => _orderType; }
        public float RemainingTime { get => _remainingTime; }
        

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
            _orderTimer = new Timer(orderConfig.OrderTime);
            _isCompleted = false;
        }

        public void PreSaveOrder()
        {
            _remainingTime = _orderTimer.GetRemainingTime();
        }
        
        public void PreLoadOrder()
        {
            _orderTimer = new Timer(_remainingTime);
        }
    }
}

