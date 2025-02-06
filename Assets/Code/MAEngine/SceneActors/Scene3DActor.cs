using System;
using UnityEngine;

namespace MAEngine
{
    public class Scene3DActor : MonoBehaviour
    {
        public Rigidbody Rigidbody;
        
        public Action<Scene3DActor, Collider> TriggerEnter;
        public Action<Scene3DActor, Collider> TriggerExit;
        public Action<Scene3DActor, Collider> TriggerStay;
        public Action<Scene3DActor, Collision> CollisionEnter;
        public Action<Scene3DActor, Collision> CollisionExit;
        public Action<Scene3DActor, Collision> CollisionStay;
        public Action<Scene3DActor> BecameVisible;
        public Action<Scene3DActor> BecameInvisible;
        
        public Vector3 GetPosition()
        {
            return gameObject.transform.position;
        }

        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }
        private void OnTriggerEnter(Collider other)
        {
            TriggerEnter?.Invoke(this, other);
        }
    
        private void OnTriggerExit(Collider other)
        {
            TriggerExit?.Invoke(this, other);
        }
    
        private void OnTriggerStay(Collider other)
        {
            TriggerStay?.Invoke(this, other);
        }
    
        private void OnCollisionEnter(Collision other)
        {
            CollisionEnter?.Invoke(this, other);
        }
    
        private void OnCollisionExit(Collision other)
        {
            CollisionExit?.Invoke(this, other);
        }
    
        private void OnCollisionStay(Collision other)
        {
            CollisionStay?.Invoke(this, other);
        }
    
        private void OnBecameVisible()
        {
            BecameVisible?.Invoke(this);
        }
        
        private void OnBecameInvisible()
        {
            BecameInvisible?.Invoke(this);
        }
    
        private void OnDisable()
        {
            TriggerEnter = null;
            TriggerExit = null;
            TriggerStay = null;
            CollisionEnter = null;
            CollisionExit = null;
            CollisionStay = null;
            BecameVisible = null;
            BecameInvisible = null;
        }
    
    }
}

