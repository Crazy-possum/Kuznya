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
        [SerializeField] private Dictionary<MaterialName, int> _materials;
        [FormerlySerializedAs("_currentMaxMaterialStorageStorage")] [SerializeField] private int _currentMaxMaterialStorage;
        [SerializeField] private float _materialAddingDeltaTime;

        public MaterialName CurrentMaximumMaterial { get => _currentMaximumMaterial; set => _currentMaximumMaterial = value; }
        public int CurrentMoney { get => _currentMoney; set => _currentMoney = value; }
        public Dictionary<MaterialName, int> Materials { get => _materials; set => _materials = value; }
        public int CurrentMaxMaterialStorage { get => _currentMaxMaterialStorage; set => _currentMaxMaterialStorage = value; }
        public float MaterialAddingDeltaTime { get => _materialAddingDeltaTime; set => _materialAddingDeltaTime = value; }

        public void Initialize()
        {
            
        }
        
        public void InitializeMaterials(StorageMaterialsConfig config)
        {
            if (_currentMaximumMaterial == MaterialName.NONE)
            {
                _currentMaximumMaterial = MaterialName.Tin;
            }
            if (_materials == null)
            {
                _materials = new Dictionary<MaterialName, int>();
                foreach (MaterialConfig materialConfig in config.StorableMaterials)
                {
                    _materials.Add(materialConfig.MaterialName, 0);
                }
            }
            else
            {
                foreach (MaterialConfig materialConfig in config.StorableMaterials)
                {
                    if (!_materials.ContainsKey(materialConfig.MaterialName))
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
    }
}
