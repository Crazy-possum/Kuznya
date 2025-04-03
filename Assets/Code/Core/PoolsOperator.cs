using MAEngine;
using MAEngine.Extention;
using UnityEngine;
using Zenject;

namespace GameCoreModule
{

    public class PoolsOperator : IAction, IInitialisation, ICleanUp
    {
        private PoolsContainer _poolsContainer;
        private GameEventBus _gameEventBus;

        [Inject]
        public void Construct(PoolsContainer poolsContainer, GameEventBus gameEventBus)
        {
            _poolsContainer = poolsContainer;
            _gameEventBus = gameEventBus;
            _poolsContainer.Initialize();
            
        }

        public void Initialisation()
        {
            _gameEventBus.OnSpawnObjectFromPool += SpawnObject;
            _gameEventBus.OnSpawnRotatedObjectFromPool += SpawnObject;
            _gameEventBus.OnCreatePool += CreatePool;
        }

        public void Cleanup()
        {
            _gameEventBus.OnSpawnObjectFromPool -= SpawnObject;
            _gameEventBus.OnSpawnRotatedObjectFromPool -= SpawnObject;
            _gameEventBus.OnCreatePool -= CreatePool;
        }
        
        public void CreatePool(PrefabID prefabID, PoolCallback poolCallback)
        {
            _poolsContainer.InitializePool(prefabID, poolCallback);
        }

        public void SpawnObject(PrefabID prefabID, Vector3 position, GameObjectSpawnCallback callback)
        {
            IPool pool = GetPool(prefabID);
            GameObject go = pool.Pop(position);
            callback.SetObject(go, pool);

        }

        private IPool GetPool(PrefabID prefabID)
        {
            IPool pool = null;
            if (_poolsContainer.PoolsDict.IsContainsKey(prefabID))
            {
                pool = _poolsContainer.PoolsDict.GetValue(prefabID);
            }
            else
            {
                PoolCallback poolCallback = new PoolCallback();
                _gameEventBus.OnCreatePool.Invoke(prefabID, poolCallback);
                pool = poolCallback.Pool;
            }
            return pool;
        }

        public void SpawnObject(PrefabID prefabID, Vector3 position, Quaternion rotation,
            GameObjectSpawnCallback callback)
        {
            IPool pool = _poolsContainer.PoolsDict.GetValue(prefabID);
            GameObject go = pool.Pop(position, rotation);
            callback.SetObject(go, pool);
        }

    }
}
