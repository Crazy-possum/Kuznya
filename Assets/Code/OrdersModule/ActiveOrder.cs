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
        private Timer _orderTimer;
        private bool _isCompleted;

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
    }
}

