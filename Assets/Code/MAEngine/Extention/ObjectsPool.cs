using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace MAEngine.Extention
{
    public class ObjectsPool : IPool
    {
        private const string DEFAULT_ROOT_NAME = "PoolRoot";
        private readonly Stack<GameObject> _stack = new Stack<GameObject>();
        private readonly GameObject _prefab;
        private readonly Transform _rootPool;
        private readonly DiContainer _di;

        public Transform Root { get => _rootPool; }
        public GameObject Prefab { get => _prefab; }

        public ObjectsPool(DiContainer diContainer, GameObject prefab)
        {
            _di = diContainer;
            _prefab = prefab;
            _rootPool = new GameObject(DEFAULT_ROOT_NAME).transform;
        }

        public ObjectsPool(DiContainer diContainer, GameObject prefab, Transform rootTransform)
        {
            _di = diContainer;
            _prefab = prefab;
            _rootPool = rootTransform;
        }

        public void Push(GameObject go)
        {
            _stack.Push(go);
            go.transform.position = _rootPool.position;
            go.transform.SetParent(_rootPool);
            go.SetActive(false);
        }

        public GameObject Pop(Vector3 position)
        {
            GameObject go;
            if (_stack.Count == 0)
            {
                go = _di.InstantiatePrefab(_prefab, position, Quaternion.identity, _rootPool);
            }
            else
            {
                go = _stack.Pop();
                go.transform.position = position;
                go.transform.rotation = Quaternion.identity;

            }
            go.SetActive(true);
            return go;
        }

        public GameObject Pop(Vector3 position, Quaternion rotation)
        {
            GameObject go;
            if (_stack.Count == 0)
            {
                go = _di.InstantiatePrefab(_prefab, position, rotation, _rootPool);
            }
            else
            {
                go = _stack.Pop();
                go.transform.position = position;
                go.transform.rotation = rotation;

            }
            go.SetActive(true);
            return go;
        }
    }
}

