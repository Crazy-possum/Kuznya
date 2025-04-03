using GameCoreModule;
using MAEngine.Extention;
using UnityEngine;
using Zenject;

namespace SaveLoad
{
    public class SavableDataContainer
    {
        private SaveLoadEventBus _saveLoadEventBus;
        
        private SerializableDictionary<SaveDataKey, int> _savableInts;
        private SerializableDictionary<SaveDataKey, float> _savableFloats;
        private SerializableDictionary<SaveDataKey, string> _savableStrings;
        
        public SerializableDictionary<SaveDataKey, int> SavableInts => _savableInts;
        public SerializableDictionary<SaveDataKey, float> SavableFloats => _savableFloats;
        public SerializableDictionary<SaveDataKey, string> SavableStrings => _savableStrings;
        
        [Inject]
        public void Construct(SaveLoadEventBus saveLoadEventBus)
        {
            _saveLoadEventBus = saveLoadEventBus;
        }

        public void Initialize()
        {
            _savableInts = new SerializableDictionary<SaveDataKey, int>();
            _savableFloats = new SerializableDictionary<SaveDataKey, float>();
            _savableStrings = new SerializableDictionary<SaveDataKey, string>();
            
            _saveLoadEventBus.OnSaveInt += SaveData;
            _saveLoadEventBus.OnSaveFloat += SaveData;
            _saveLoadEventBus.OnSaveString += SaveData;
            _saveLoadEventBus.OnGetInt += LoadData;
            _saveLoadEventBus.OnGetFloat += LoadData;
            _saveLoadEventBus.OnGetString += LoadData;
        }
        
        public void CleanUp()
        {
            _saveLoadEventBus.OnSaveInt -= SaveData;
            _saveLoadEventBus.OnSaveFloat -= SaveData;
            _saveLoadEventBus.OnSaveString -= SaveData;
            _saveLoadEventBus.OnGetInt -= LoadData;
            _saveLoadEventBus.OnGetFloat -= LoadData;
            _saveLoadEventBus.OnGetString -= LoadData;
        }
        
        
        public void SaveData(SaveDataKey key, int value)
        {
            if (_savableInts.IsContainsKey(key))
            {
                _savableInts[key] = value;
            }
            else
            {
                _savableInts.Add(key, value);
            }
            _saveLoadEventBus.OnSavableDataChanged?.Invoke();
        }
        
        public void SaveData(SaveDataKey key, float value)
        {
            if (_savableFloats.IsContainsKey(key))
            {
                _savableFloats[key] = value;
            }
            else
            {
                _savableFloats.Add(key, value);
            }
            _saveLoadEventBus.OnSavableDataChanged?.Invoke();
        }
        
        public void SaveData(SaveDataKey key, string value)
        {
            if (_savableStrings.IsContainsKey(key))
            {
                _savableStrings[key] = value;
            }
            else
            {
                _savableStrings.Add(key, value);
            }
            _saveLoadEventBus.OnSavableDataChanged?.Invoke();
        }

        private void LoadData(SaveDataKey key, SavableInt savable)
        {
            if (_savableInts.IsContainsKey(key))
            {
                savable.SetValue(_savableInts[key]);
            }

            //Debug.LogWarning($"No int saved by key {key}");
        }
        
        private void LoadData(SaveDataKey key, SavableFloat savable)
        {
            if (_savableFloats.IsContainsKey(key))
            {
                savable.SetValue(_savableFloats[key]);
            }

            //Debug.LogWarning($"No float saved by key {key}");
        }

        private void LoadData(SaveDataKey key, SavableString savable)
        {
            if (_savableStrings.IsContainsKey(key))
            {
                savable.SetValue(_savableStrings[key]);
            }

            //Debug.LogWarning($"No string saved by key {key}");
        }
    }
}