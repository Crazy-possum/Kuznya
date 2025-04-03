using System.Collections.Generic;
using UnityEngine;

namespace SaveLoad
{
    [CreateAssetMenu(fileName = "SaveDataKeysContainer", menuName = "SO/Containers/SaveDataKeysContainer" )]
    public class SaveDataKeysContainer : ScriptableObject
    {
        [SerializeField] private List<SaveDataKey> _savableIntKeys;
        [SerializeField] private List<SaveDataKey> _savableFloatKeys;
        [SerializeField] private List<SaveDataKey> _savableStringKeys;
        
        public List<SaveDataKey> GetSavableIntKeys() => _savableIntKeys;
        public List<SaveDataKey> GetSavableFloatKeys() => _savableFloatKeys;
        public List<SaveDataKey> GetSavableStringKeys() => _savableStringKeys;
    }
}