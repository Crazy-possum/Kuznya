using System;
using UnityEngine;

namespace MAEngine.Extention
{
    [Serializable]
    public class Vector2Seralizable
    {
        [SerializeField] private float _x;
        [SerializeField] private float _y;

        public float X => _x;
        public float Y => _y;

        public Vector2Seralizable(Vector2 vector)
        {
            _x = vector.x;
            _y = vector.y;
        }
    
        public Vector2Seralizable(float x, float y)
        {
            _x = x;
            _y = y;
        }
    }
}
