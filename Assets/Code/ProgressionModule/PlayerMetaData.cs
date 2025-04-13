using System;
using System.Collections.Generic;
using MAEngine.Extention;
using UnityEngine;
using UnityEngine.Serialization;

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

        public void Initialize()
        {
            
        }
        
        public void InitializeMaterials(StorageMaterialsConfig config)
        {
            _storageMaterialsConfig = config;
            if (_currentMaximumMaterial == MaterialName.NONE)
            {
                _currentMaximumMaterial = MaterialName.Tin;
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
    }
}
