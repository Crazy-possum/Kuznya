using System;
using System.Collections.Generic;
using UnityEngine;

namespace MAEngine.Extention
{
    public static class EngineExtention
    {
        #region Vector

        public static Vector3 MultiplyX(this Vector3 v, float val)
        {
            v = new Vector3(val * v.x, v.y, v.z);
            return v;
        }

        public static Vector3 MultiplyY(this Vector3 v, float val)
        {
            v = new Vector3(v.x, val * v.y, v.z);
            return v;
        }

        public static Vector3 MultiplyZ(this Vector3 v, float val)
        {
            v = new Vector3(v.x, v.y, val * v.z);
            return v;
        }

        public static float GetDistanceBetweenPoints(Vector2 pointA, Vector2 pointB)
        {
            float distance = Mathf.Sqrt(Mathf.Pow((pointB.x - pointA.x), 2) + Mathf.Pow((pointB.y - pointA.y), 2));
            return distance;
        }

        public static float GetDistanceBetweenPoints(Vector3 pointA, Vector3 pointB)
        {
            float distance = Mathf.Sqrt(Mathf.Pow((pointB.x - pointA.x), 2) + Mathf.Pow((pointB.y - pointA.y), 2) + Mathf.Pow((pointB.z - pointA.z), 2));
            return distance;
        }

        #endregion

        #region Components

        public static T GetOrAddComponent<T>(this GameObject child) where T : Component
        {
            T result = child.GetComponent<T>();
            if (result == null)
            {
                result = child.AddComponent<T>();
            }

            return result;
        }
        
        public static bool TryGetComponentInChildren<T>(this GameObject child, out T component) where T : Component
        {
            component = child.GetComponentInChildren<T>();
            return component != null;
        }

        public static GameObject SetName(this GameObject gameObject, string name)
        {
            gameObject.name = name;
            return gameObject;
        }

        public static GameObject AddRigidBody2D(this GameObject gameObject, float mass)
        {
            var component = gameObject.GetOrAddComponent<Rigidbody2D>();
            component.mass = mass;
            return gameObject;
        }

        public static GameObject AddRigidBody(this GameObject gameObject, float mass)
        {
            var component = gameObject.GetOrAddComponent<Rigidbody>();
            component.mass = mass;
            return gameObject;
        }

        public static GameObject AddBoxCollider2D(this GameObject gameObject)
        {
            var component = gameObject.GetOrAddComponent<BoxCollider2D>();
            return gameObject;
        }

        public static GameObject AddBoxCollider(this GameObject gameObject)
        {
            var component = gameObject.GetOrAddComponent<BoxCollider>();
            return gameObject;
        }

        public static GameObject AddCircleCollider2D(this GameObject gameObject)
        {
            var component = gameObject.GetOrAddComponent<CircleCollider2D>();
            return gameObject;
        }

        public static GameObject AddSphereCollider(this GameObject gameObject)
        {
            var component = gameObject.GetOrAddComponent<SphereCollider>();
            return gameObject;
        }

        public static GameObject AddSprite(this GameObject gameObject, Sprite sprite)
        {
            var component = gameObject.GetOrAddComponent<SpriteRenderer>();
            component.sprite = sprite;
            return gameObject;
        }

        public static GameObject AddMesh(this GameObject gameObject, Material material)
        {
            var component = gameObject.GetOrAddComponent<MeshRenderer>();
            component.material = material;
            return gameObject;
        }

        #endregion
        
        #region ObjectsList

        public static List<GameObject> GetAllChildsOfObjects(this List<GameObject> list)
        {
            List<GameObject> childsList = new List<GameObject>();
            foreach (GameObject listObject in list)
            {
                foreach (Transform childTransform in listObject.transform.GetComponentsInChildren<Transform>())
                {
                    childsList.Add(childTransform.gameObject);
                }
            }
            return childsList;
        }
        
        public static GameObject GetObjectByTag(this List<GameObject> list, string tag)
        {
            foreach (GameObject listObject in list)
            {
                if (listObject.tag == tag)
                {
                    return listObject;
                }
            }
            return null;
        }
        
        public static GameObject GetObjectByTagInChildren(this List<GameObject> list, string tag)
        {
            List<GameObject> childsList = list.GetAllChildsOfObjects();
            foreach (GameObject listObject in childsList)
            {
                if (listObject.tag == tag)
                {
                    return listObject;
                }
            }
            return null;
        }
        
        public static GameObject GetObjectByName(this List<GameObject> list, string name)
        {
            foreach (GameObject listObject in list)
            {
                if (listObject.name == name)
                {
                    return listObject;
                }
            }
            return null;
        }
        
        public static GameObject GetObjectByNameInChildren(this List<GameObject> list, string name)
        {
            List<GameObject> childsList = list.GetAllChildsOfObjects();
            foreach (GameObject listObject in childsList)
            {
                if (listObject.name == name)
                {
                    return listObject;
                }
            }
            return null;
        }
        
        public static GameObject GetObjectByTagInComponentsList<T>(this List<T> list, string tag) where T : Component
        {
            foreach (T listObject in list)
            {
                if (listObject.tag == tag)
                {
                    return listObject.gameObject;
                }
            }
            return null;
        }
        
        public static GameObject GetObjectByNameInComponentsList<T>(this List<T> list, string name) where T : Component
        {
            foreach (T listObject in list)
            {
                if (listObject.name == name)
                {
                    return listObject.gameObject;
                }
            }
            return null;
        }
        
        public static T GetComponentByObjectTag<T>(this List<T> list, string tag) where T : Component
        {
            foreach (T listObject in list)
            {
                if (listObject.tag == tag)
                {
                    return listObject;
                }
            }
            return null;
        }

        public static T GetComponentByObjectName<T>(this List<T> list, string name) where T : Component
        {
            foreach (T listObject in list)
            {
                if (listObject.name == name)
                {
                    return listObject;
                }
            }
            return null;
        }

        public static int CountActiveChild(this Transform transform)
        {
            int k = 0;
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                    k++;
            }
            return k;
        }

        #endregion

        #region Vector according Camera

        public static Vector3 GetCentrAccordingCamera(this Camera camera)
        {
            var centrPosition = new Vector3(camera.transform.position.x, camera.transform.position.y, 0);
            return centrPosition;
        }

        public static Vector3 GetRandomVectorAccordingCamera(this Camera camera, float offsetX, float offsetY)
        {
            var weight = camera.orthographicSize * 2 * camera.aspect / 2;
            var height = camera.orthographicSize;
            var randomX = UnityEngine.Random.Range(-1 * weight + offsetX, weight - offsetX);
            var vector = new Vector2(randomX, Mathf.Abs(height) - offsetY);
            return vector;
        }

        public static Vector2 GetLeftSideVector2AccordingCamera(this Camera camera, float offset)
        {
            var vector_left = new Vector2(camera.transform.localPosition.z + offset, camera.transform.localPosition.y);
            return vector_left;
        }

        public static float GetLeftSideValueAccordingCamera(this Camera camera, float offset)
        {
            var left = camera.transform.localPosition.z + offset;
            return left;
        }

        public static Vector2 GetRightSideVector2AccordingCamera(this Camera camera, float offset)
        {
            var right = (camera.transform.localPosition.z + offset) * -1;
            var vector_right = new Vector2(right, camera.transform.localPosition.y);
            return vector_right;
        }

        public static float GetRightSideValueAccordingCamera(this Camera camera, float offset)
        {
            var right = (camera.transform.localPosition.z + offset) * -1;
            return right;
        }

        public static float GetUpSideValueAccordingCamera(this Camera camera, float offset)
        {
            var up = (camera.transform.localPosition.z / 2 + offset) * -1;
            return up;
        }

        public static float GetDownSideValueAccordingCamera(this Camera camera, float offset)
        {
            var down = camera.transform.localPosition.z / 2 + offset;
            return down;
        }

        #endregion

        #region String

        public static List<string> SplitBy(this string str, int chunkLength)
        {
            List<string> strings = new List<string>();
            if (String.IsNullOrEmpty(str)) throw new ArgumentException();
            if (chunkLength < 1) throw new ArgumentException();

            for (int i = 0; i < str.Length; i += chunkLength)
            {
                if (chunkLength + i > str.Length)
                    chunkLength = str.Length - i;

                strings.Add(str.Substring(i, chunkLength));
            }

            return strings;
        }

        #endregion
    }
}