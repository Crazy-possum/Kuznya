using MAEngine.Extention;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Orders
{
    [Serializable]
    public class ActiveOrder
    {
        private string _name;
        private OrderText _description;
        private Sprite _orderIcon;
        private List<ForgingMaterial> _materials;
        private float _orderTime;
        private Timer _orderTimer;
        private bool _isCompleted;

        public string Name { get => _name; }
        public OrderText Description { get => _description; }
        public Sprite OrderIcon { get => _orderIcon; }
        public List<ForgingMaterial> Materials { get => _materials; }
        public float OrderTime { get => _orderTime; }
        public Timer OrderTimer { get => _orderTimer; set => _orderTimer = value; }
        public bool IsCompleted { get => _isCompleted; set => _isCompleted = value; }
        

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

        public ActiveOrder(OrderConfig orderConfig, int DecriptionID)
        {
            _name = orderConfig.Name;
            _description = orderConfig.Descriptions[DecriptionID];
            _orderIcon = orderConfig.OrderIcon;
            _materials = orderConfig.Materials;
            _orderTime = orderConfig.OrderTime;
            _orderTimer = new Timer(orderConfig.OrderTime);
            _isCompleted = false;
        }
    }
}

