using System;
using UnityEngine;

namespace Orders
{
    [Serializable]
    public class ForgingMaterial
    {
        [SerializeField] private MaterialConfig _config;
        [SerializeField] private int _count;

        public MaterialConfig Config { get => _config; }
        public int Count { get => _count; }
    }
}

