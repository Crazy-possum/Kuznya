using MAEngine.Extention;
using Zenject;
using SO;
using UnityEngine;
using MAEngine;

namespace GameCoreModule
{
    public class PoolsContainer
    {
        private DiContainer _di;
        private PrefabsContainer _prefabsContainer;
        
        private SerializableDictionary<PrefabID, IPool> _poolsDict;

        public SerializableDictionary<PrefabID, IPool> PoolsDict { get => _poolsDict; }

        [Inject]
        public void Construct(DiContainer di, PrefabsContainer prefabsContainer)
        {
            _di = di;
            _prefabsContainer = prefabsContainer;
        }

        public void Initialize()
        {
            _poolsDict = new SerializableDictionary<PrefabID, IPool>();
        }

        public void InitializePool(PrefabID prefabID, PoolCallback callback)
        {
            GameObject prefab = _prefabsContainer.GetPrefab(prefabID);
            Transform poolRoot = new GameObject(prefabID.ToString()).transform;
            ObjectsPool pool = new ObjectsPool(_di, prefab, poolRoot);
            _poolsDict.Add(prefabID, pool);
            callback.SetPool(pool);
        }
    }
}
