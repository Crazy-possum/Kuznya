using System;
using UnityEngine;

namespace Orders
{
    [Serializable]
    public class OrderText
    {
        [SerializeField] private string _text;
        [SerializeField] private string _description;

        public string Text { get => _text; }
        public string Description { get => _description; }
    }
}

