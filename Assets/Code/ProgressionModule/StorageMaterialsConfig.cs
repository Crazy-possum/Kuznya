using System.Collections.Generic;
using UnityEngine;

namespace Progression
{
    [CreateAssetMenu(fileName = "StorageMaterialsConfig", menuName = "Configs/StorageMaterialsConfig")]

    public class StorageMaterialsConfig : ScriptableObject
    {
        [SerializeField] private List<MaterialConfig> _storableMaterials;
        [SerializeField] private List<int> _storageCapacity;
        [SerializeField] private List<float> _materialAddingDeltaTime;
        
        public List<MaterialConfig> StorableMaterials { get => _storableMaterials; set => _storableMaterials = value; }
        public List<int> StorageCapacity { get => _storageCapacity; set => _storageCapacity = value; }
        public List<float> MaterialAddingDeltaTime { get => _materialAddingDeltaTime; set => _materialAddingDeltaTime = value; }
    }
}