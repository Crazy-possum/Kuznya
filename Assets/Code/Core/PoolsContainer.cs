using MAEngine.Extention;
using Zenject;
using UnityEngine;
using MAEngine;

namespace GameCoreModule
{
    public class PoolsContainer
    {
        private DiContainer _di;
        private PrefabsContainer _prefabsContainer;
        private GameEventBus _gameEventBus;
        private SerializableDictionary<PrefabID, IPool> _poolsDict;

        public SerializableDictionary<PrefabID, IPool> PoolsDict { get => _poolsDict; }

        [Inject]
        public void Construct(DiContainer di, PrefabsContainer prefabsContainer, GameEventBus gameEventBus)
        {
            _di = di;
            _prefabsContainer = prefabsContainer;
            _gameEventBus = gameEventBus;
        }

        public void Initialize()
        {
            _poolsDict = new SerializableDictionary<PrefabID, IPool>();
            _gameEventBus.OnCreatePool += InitializePool;
        }

        public void CleanUp()
        {
            _gameEventBus.OnCreatePool -= InitializePool;

        }

        private void InitializePool(PrefabID prefabID)
        {
            GameObject prefab = _prefabsContainer.PrefabsDict.GetValue(prefabID);
            Transform poolRoot = new GameObject(prefabID.ToString()).transform;
            ObjectsPool pool = new ObjectsPool(_di, prefab, poolRoot);
            _poolsDict.Add(prefabID, pool);
        }

        private void InitializePool(PrefabID prefabID, Transform poolRoot)
        {
            GameObject prefab = _prefabsContainer.PrefabsDict.GetValue(prefabID);
            ObjectsPool pool = new ObjectsPool(_di, prefab, poolRoot);
            _poolsDict.Add(prefabID, pool);
        }
    }
}
