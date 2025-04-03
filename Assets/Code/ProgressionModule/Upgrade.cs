using System;
using UnityEngine;

namespace Progression
{
    [Serializable]
    public class Upgrade
    {
        [SerializeField] private float _upgradeModifier;
        [SerializeField] private bool _isUpgradeActive;

        public bool IsUpgradeActive => _isUpgradeActive;

        public void SetUpgradeModifier(float value)
        {
            _isUpgradeActive = true;
            _upgradeModifier = value;
        }

        public void ActivateUpgrade()
        {
            _isUpgradeActive = true;
        }

        public float GetUpgradeData()
        {
            if (_upgradeModifier != 0)
            {
                return _upgradeModifier;
            }
            else
            {
                return 0;
            }
        }

        public void Clear()
        {
            _upgradeModifier = 0;
            _isUpgradeActive = false;
        }
    }
}