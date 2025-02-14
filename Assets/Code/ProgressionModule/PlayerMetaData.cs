using System;
using UnityEngine;

namespace Progression
{
    [Serializable]
    public class PlayerMetaData
    {
        [SerializeField] private MaterialName _currentMaximumMaterial;
        [SerializeField] private int _currentMoney;

        public MaterialName CurrentMaximumMaterial { get => _currentMaximumMaterial; set => _currentMaximumMaterial = value; }
        public int CurrentMoney { get => _currentMoney; set => _currentMoney = value; }
    }
}
