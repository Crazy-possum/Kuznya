using System;
using MAEngine.Extention;
using UnityEngine;

namespace Progression
{
    [Serializable]
    public class UpgradesMetaData
    {
        [SerializeField] private SerializableDictionary<UpgradeName, int> _upgradesDict;

        [SerializeField] private SerializableDictionary<UpgradeName, Upgrade> _upgrades;
        
        public SerializableDictionary<UpgradeName, int> UpgradesDict => _upgradesDict;
        
        public SerializableDictionary<UpgradeName, Upgrade> Upgrades => _upgrades;

        public void SetUpgradeLevel(UpgradeName upgradeName, int upgradeLevel)
        {
            if (!_upgradesDict.IsContainsKey(upgradeName))
            {
                _upgradesDict.Add(upgradeName, upgradeLevel);
            }
            else
            {
                _upgradesDict[upgradeName] = upgradeLevel;
            }
        }
        
        public int GetUpgradeLevel(UpgradeName upgradeName)
        {
            if (_upgradesDict.IsContainsKey(upgradeName))
            {
                return _upgradesDict[upgradeName];
            }
            else
            {
                //Debug.Log($"No saved upgrade with name {upgradeName} found");
                return 0;
            }
        }
        
        public void Initialize()
        {
            _upgradesDict = new SerializableDictionary<UpgradeName, int>();
        }
        
        public void Clear()
        {
            if (_upgradesDict != null)
            {
                _upgradesDict.Clear();
            }

            if (_upgrades != null)
            {
                _upgrades.Clear();
            }
        }
    }
}