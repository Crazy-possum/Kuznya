using MAEngine;
using UnityEngine;

namespace GameCoreModule
{
    public class GameObjectSpawnCallback
    {
        private GameObject _spawnedObject;
        private IPool _pool;
        
        public GameObject SpawnedObject => _spawnedObject;
        public IPool Pool => _pool;
        
        public void SetObject(GameObject spawnedObject)
        {
            _spawnedObject = spawnedObject;
        }
        public void SetObject(GameObject spawnedObject, IPool pool)
        {
            _spawnedObject = spawnedObject;
            _pool = pool;
        }
    }
}