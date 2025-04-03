using System.Collections.Generic;
using GameCoreModule;
using MAEngine;
using UnityEngine;
using Zenject;

namespace SaveLoad
{
    public class SaveLoadActions : IAction, IInitialisation, ICleanUp, IPreInitialisation
    {
        private SaveLoadEventBus _saveLoadEventBus;
        private SavableDataContainer _savableDataContainer;
        
        private SaveDataKeysContainer _saveDataKeysContainer;
        
        
        [Inject]
        public void Construct(SaveLoadEventBus saveLoadEventBus, SavableDataContainer saveableDataContainer,
            SaveDataKeysContainer saveDataKeysContainer)
        {
            _saveLoadEventBus = saveLoadEventBus;
            _savableDataContainer = saveableDataContainer;
            _saveDataKeysContainer = saveDataKeysContainer;
        }

        public void PreInitialisation()
        {
            _savableDataContainer.Initialize();
            //_saveLoadEventBus.OnSavableDataChanged += SaveAllDataToPlayerPrefs;
            _saveLoadEventBus.OnSaveData += SaveAllDataToPlayerPrefs;
            _saveLoadEventBus.OnLoadData += LoadAllDataFromPlayerPrefs;
            _saveLoadEventBus.OnLoadData?.Invoke();
        }

        public void Initialisation()
        {
            
        }

        public void Cleanup()
        {
            _savableDataContainer.CleanUp();
            //_saveLoadEventBus.OnSavableDataChanged -= SaveAllDataToPlayerPrefs;
            _saveLoadEventBus.OnSaveData -= SaveAllDataToPlayerPrefs;
            _saveLoadEventBus.OnLoadData -= LoadAllDataFromPlayerPrefs;
        }

        public void SaveAllDataToPlayerPrefs()
        {
            List<SaveDataKey> saveDataKeys = _savableDataContainer.SavableInts.GetAllKeys();
            foreach (SaveDataKey key in saveDataKeys)
            {
                PlayerPrefs.SetInt(key.ToString(), _savableDataContainer.SavableInts[key]);
            }
            saveDataKeys = _savableDataContainer.SavableFloats.GetAllKeys();
            foreach (SaveDataKey key in saveDataKeys)
            {
                PlayerPrefs.SetFloat(key.ToString(), _savableDataContainer.SavableFloats[key]);
            }
            saveDataKeys = _savableDataContainer.SavableStrings.GetAllKeys();
            foreach (SaveDataKey key in saveDataKeys)
            {
                PlayerPrefs.SetString(key.ToString(), _savableDataContainer.SavableStrings[key]);
            }
        }

        public void LoadAllDataFromPlayerPrefs()
        {
            List<SaveDataKey> saveDataKeys = _saveDataKeysContainer.GetSavableIntKeys();
            foreach (SaveDataKey key in saveDataKeys)
            {
                if (PlayerPrefs.HasKey(key.ToString()))
                {
                    _savableDataContainer.SaveData(key, PlayerPrefs.GetInt(key.ToString()));
                }
            }
            saveDataKeys = _saveDataKeysContainer.GetSavableFloatKeys();
            foreach (SaveDataKey key in saveDataKeys)
            {
                if (PlayerPrefs.HasKey(key.ToString()))
                {
                    _savableDataContainer.SaveData(key, PlayerPrefs.GetFloat(key.ToString()));
                    Debug.Log(PlayerPrefs.GetFloat(key.ToString()));
                }
            }
            saveDataKeys = _saveDataKeysContainer.GetSavableStringKeys();
            foreach (SaveDataKey key in saveDataKeys)
            {
                if (PlayerPrefs.HasKey(key.ToString()))
                {
                    _savableDataContainer.SaveData(key, PlayerPrefs.GetString(key.ToString()));
                }
            }
        }


    }
}