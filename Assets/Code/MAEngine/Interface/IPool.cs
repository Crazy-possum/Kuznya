using UnityEngine;

namespace MAEngine
{
    public interface IPool
    {
        public Transform Root { get; }
        public GameObject Prefab { get; }
        public GameObject Pop(Vector3 position);
        public GameObject Pop(Vector3 position, Quaternion rotation);
        public void Push(GameObject go);
    }

}
