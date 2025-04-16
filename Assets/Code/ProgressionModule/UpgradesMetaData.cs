using System;
using GameCoreModule;
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
        
        public void Initialize(SaveLoadEventBus saveLoadEventBus)
        {
            _upgradesDict = new SerializableDictionary<UpgradeName, int>();
            LoadData(saveLoadEventBus);
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

        public void SaveData(SaveLoadEventBus saveLoadEventBus)
        {
            saveLoadEventBus.OnSaveString?.Invoke(SaveDataKey.UpgradesDict, _upgradesDict.ToString());
            saveLoadEventBus.OnSaveString?.Invoke(SaveDataKey.Upgrades, _upgrades.ToString());
        }

        public void LoadData(SaveLoadEventBus saveLoadEventBus)
        {
            SavableString savableString = new SavableString();
            saveLoadEventBus.OnGetString?.Invoke(SaveDataKey.UpgradesDict, savableString);
            if (savableString.IsLoaded)
            {
                string upgradesDictString = savableString.Value;
                SetupUpgradesDict(upgradesDictString);
            }
            else
            {
                _upgradesDict = new SerializableDictionary<UpgradeName, int>();
            }
            savableString = new SavableString();
            saveLoadEventBus.OnGetString?.Invoke(SaveDataKey.Upgrades, savableString);
            if (savableString.IsLoaded)
            {
                string upgradesString = savableString.Value;
                SetupUpgrades(upgradesString);
            }
            else
            {
                _upgrades = new SerializableDictionary<UpgradeName, Upgrade>();
            }
        }

        private void SetupUpgradesDict(string upgradesDictString)
        {
            if (!string.IsNullOrEmpty(upgradesDictString))
            {
                _upgradesDict = new SerializableDictionary<UpgradeName, int>();
                string[] upgradesDictStrings = upgradesDictString.Split("],[", StringSplitOptions.RemoveEmptyEntries);
                foreach (string upgradeDictElement in upgradesDictStrings)
                {
                    string[] items = upgradeDictElement.Split(" : " , StringSplitOptions.RemoveEmptyEntries);
                    if (items.Length == 2)
                    {
                        UpgradeName upgradeName = (UpgradeName)Enum.Parse(typeof(UpgradeName), items[0]);
                        int upgradeLevel = int.Parse(items[1]);
                        _upgradesDict.Add(upgradeName, upgradeLevel);
                    }
                }
            }
            else
            {
                _upgradesDict = new SerializableDictionary<UpgradeName, int>();
            }
        }
        
        private void SetupUpgrades(string upgradesString)
        {
            if (!string.IsNullOrEmpty(upgradesString))
            {
                _upgrades = new SerializableDictionary<UpgradeName, Upgrade>();
                string[] upgradesStrings = upgradesString.Split("],[", StringSplitOptions.RemoveEmptyEntries);
                foreach (string upgradeElement in upgradesStrings)
                {
                    string[] items = upgradeElement.Split(" : " , StringSplitOptions.RemoveEmptyEntries);
                    if (items.Length == 2)
                    {
                        UpgradeName upgradeName = (UpgradeName)Enum.Parse(typeof(UpgradeName), items[0]);
                        Upgrade upgrade = new Upgrade();
                        upgrade.LoadUpgrade(items[1]);
                        _upgrades.Add(upgradeName, upgrade);
                    }
                }
            }
            else
            {
                _upgrades = new SerializableDictionary<UpgradeName, Upgrade>();
            }
        }
    }
}