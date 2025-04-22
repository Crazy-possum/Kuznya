using System;
using GameCoreModule;
using MAEngine.Extention;
using UnityEngine;

namespace Progression
{
    [Serializable]
    public class PlayerMetaData
    {
        [SerializeField] private MaterialName _currentMaximumMaterial;
        [SerializeField] private int _currentMoney;
        [SerializeField] private SerializableDictionary<MaterialName, int> _materials;
        [SerializeField] private int _currentMaxMaterialStorage;
        [SerializeField] private float _materialAddingDeltaTime;

        private StorageMaterialsConfig _storageMaterialsConfig;

        public MaterialName CurrentMaximumMaterial { get => _currentMaximumMaterial; set => _currentMaximumMaterial = value; }
        public int CurrentMoney { get => _currentMoney; set => _currentMoney = value; }
        public SerializableDictionary<MaterialName, int> Materials { get => _materials; set => _materials = value; }
        public int CurrentMaxMaterialStorage { get => _currentMaxMaterialStorage; set => _currentMaxMaterialStorage = value; }
        public float MaterialAddingDeltaTime { get => _materialAddingDeltaTime; set => _materialAddingDeltaTime = value; }
        public StorageMaterialsConfig StorageMaterialsConfig { get => _storageMaterialsConfig; set => _storageMaterialsConfig = value; }

        public void Initialize(SaveLoadEventBus saveLoadEventBus)
        {
            LoadData(saveLoadEventBus);
        }

        public void InitializeMaterials(StorageMaterialsConfig config)
        {
            _storageMaterialsConfig = config;
            if (_currentMaximumMaterial == MaterialName.NONE)
            {
                _currentMaximumMaterial = MaterialName.Metal;
            }
            if (_materials == null)
            {
                _materials = new SerializableDictionary<MaterialName, int>();
                foreach (MaterialConfig materialConfig in config.StorableMaterials)
                {
                    _materials.Add(materialConfig.MaterialName, 0);
                }
            }
            else
            {
                foreach (MaterialConfig materialConfig in config.StorableMaterials)
                {
                    if (!_materials.IsContainsKey(materialConfig.MaterialName))
                    {
                        _materials.Add(materialConfig.MaterialName, 0);
                    }
                }
            }

            if (_currentMaxMaterialStorage == 0)
            {
                _currentMaxMaterialStorage = config.StorageCapacity[0];
            }

            if (_materialAddingDeltaTime == 0)
            {
                _materialAddingDeltaTime = config.MaterialAddingDeltaTime[0];
            }
        }

        public void Clear()
        {
            _currentMaximumMaterial = MaterialName.Metal;
            _currentMoney = 0;
            _materials.Clear();
            if (_storageMaterialsConfig != null)
            {
                _currentMaxMaterialStorage = _storageMaterialsConfig.StorageCapacity[0];
                _materialAddingDeltaTime = _storageMaterialsConfig.MaterialAddingDeltaTime[0];
            }
            else
            {
                _currentMaxMaterialStorage = 0;
                _materialAddingDeltaTime = 0;
            }
        }

        public void SaveData(SaveLoadEventBus saveLoadEventBus)
        {
            saveLoadEventBus.OnSaveString?.Invoke(SaveDataKey.CurrentMaximumMaterial, _currentMaximumMaterial.ToString());
            saveLoadEventBus.OnSaveInt?.Invoke(SaveDataKey.CurrentMoney, _currentMoney);
            saveLoadEventBus.OnSaveString?.Invoke(SaveDataKey.Materials, _materials.ToString());
            saveLoadEventBus.OnSaveInt?.Invoke(SaveDataKey.CurrentMaxMaterialStorage, _currentMaxMaterialStorage);
            saveLoadEventBus.OnSaveFloat?.Invoke(SaveDataKey.MaterialAddingDeltaTime, _materialAddingDeltaTime);
        }
        
        private void LoadData(SaveLoadEventBus saveLoadEventBus)
        {
            SavableString savableString = new SavableString();
            saveLoadEventBus.OnGetString?.Invoke(SaveDataKey.CurrentMaximumMaterial, savableString);
            if (savableString.IsLoaded)
            {
                string currentMaximumMaterialString = savableString.Value;
                _currentMaximumMaterial = currentMaximumMaterialString.ParseEnum<MaterialName>();
            }
            else
            {
                _currentMaximumMaterial = MaterialName.Metal;
            }
            SavableInt savableInt = new SavableInt();
            saveLoadEventBus.OnGetInt?.Invoke(SaveDataKey.CurrentMoney, savableInt);
            if (savableInt.IsLoaded)
            {
                _currentMoney = savableInt.Value;
            }
            else
            {
                _currentMoney = 0;
            }
            savableString = new SavableString();
            saveLoadEventBus.OnGetString?.Invoke(SaveDataKey.Materials, savableString);
            if (savableString.IsLoaded)
            {
                string materialsString = savableString.Value;
                SetupMaterials(materialsString);
            }
            else
            {
                _materials = new SerializableDictionary<MaterialName, int>();
            }
            savableInt = new SavableInt();
            saveLoadEventBus.OnGetInt?.Invoke(SaveDataKey.CurrentMaxMaterialStorage, savableInt);
            if (savableInt.IsLoaded)
            {
                _currentMaxMaterialStorage = savableInt.Value;
            }
            else
            {
                _currentMaxMaterialStorage = 0;
            }
            SavableFloat savableFloat = new SavableFloat();
            saveLoadEventBus.OnGetFloat(SaveDataKey.MaterialAddingDeltaTime, savableFloat);
            if (savableFloat.IsLoaded)
            {
                _materialAddingDeltaTime = savableFloat.Value;
            }
            else
            {
                _materialAddingDeltaTime = 0;
            }
        }

        private void SetupMaterials(string materialsString)
        {
            string[] materialsArray = materialsString.Split("],[", StringSplitOptions.RemoveEmptyEntries);
            _materials = new SerializableDictionary<MaterialName, int>();
            foreach (string materialString in materialsArray)
            {
                string[] items = materialString.Split(" : ", StringSplitOptions.RemoveEmptyEntries);
                if (items.Length == 2)
                {
                    MaterialName materialName = (MaterialName)Enum.Parse(typeof(MaterialName), items[0]);
                    int materialAmount = int.Parse(items[1]);
                    _materials.Add(materialName, materialAmount);
                }
            }
        }
    }
}
