using UnityEngine;
using MAEngine.Extention;

namespace SO
{
    [CreateAssetMenu(fileName = "PrefabsContainer", menuName = "SO/Containers/PrefabsContainer", order = 0)]
    public class PrefabsContainer : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<PrefabID, GameObject> _prefabsDict;

        public SerializableDictionary<PrefabID, GameObject> PrefabsDict { get => _prefabsDict; }

        public GameObject GetPrefab(PrefabID id)
        {
            GameObject prefab = _prefabsDict[id];
            if (prefab == null)
            {
                Debug.LogWarning($"No prefab with ID : {id}");
            }

            return prefab;
        }
    }
}
