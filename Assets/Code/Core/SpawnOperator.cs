using MAEngine;
using UnityEngine;
using Zenject;
using SO;

namespace GameCoreModule
{
    public class SpawnOperator : IAction, IPreInitialisation, ICleanUp
    {
        private DiContainer _di;
        private PrefabsContainer _prefabsContainer;
        private GameEventBus _eventBus;
        private SceneViewsContainer _sceneViewsContainer;

        [Inject]
        public void Construct(DiContainer diContainer, PrefabsContainer prefabsContainer,
            GameEventBus gameEventBus, SceneViewsContainer sceneViewsContainer)
        {
            _di = diContainer;
            _prefabsContainer = prefabsContainer;
            _eventBus = gameEventBus;
            _sceneViewsContainer = sceneViewsContainer;
        }

        public void PreInitialisation()
        {
            _eventBus.OnSpawnObject += SpawnObject;
            _eventBus.OnSpawnObjectWithoutRoot += SpawnObject;
            _eventBus.OnSpawnRotatedObject += SpawnObject;
        }

        public void Cleanup()
        {
            _eventBus.OnSpawnObject -= SpawnObject;
            _eventBus.OnSpawnObjectWithoutRoot -= SpawnObject;
            _eventBus.OnSpawnRotatedObject -= SpawnObject;
        }

        private void SpawnObject(PrefabID prefabID, Vector3 position, GameObjectSpawnCallback callback)
        {
            GameObject prefab = _prefabsContainer.GetPrefab(prefabID);
            GameObject go = _di.InstantiatePrefab(prefab);
            go.transform.position = position;
            go.name = prefab.name;
            IView view;
            if (go.TryGetComponent<IView>(out view))
            {
                _sceneViewsContainer.AddView(view);
            }
            callback.SetObject(go);
        }

        private void SpawnObject(PrefabID prefabID, Vector3 position, Transform root,
            GameObjectSpawnCallback callback)
        {
            GameObject prefab = _prefabsContainer.GetPrefab(prefabID);
            GameObject go = _di.InstantiatePrefab(prefab, position, Quaternion.identity, root);
            go.name = prefab.name;
            IView view;
            if (go.TryGetComponent<IView>(out view))
            {
                _sceneViewsContainer.AddView(view);
            }
            callback.SetObject(go);
        }

        private void SpawnObject(PrefabID prefabID, Vector3 position, Quaternion rotation, Transform root,
            GameObjectSpawnCallback callback)
        {
            GameObject prefab = _prefabsContainer.GetPrefab(prefabID);
            GameObject go = _di.InstantiatePrefab(prefab, position, rotation, root);
            go.name = prefab.name;
            IView view;
            if (go.TryGetComponent<IView>(out view))
            {
                _sceneViewsContainer.AddView(view);
            }
            callback.SetObject(go);
        }
    }
}

