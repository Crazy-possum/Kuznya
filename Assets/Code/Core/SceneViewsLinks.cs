using UnityEngine;
using System.Collections.Generic;

namespace GameCoreModule
{
    public class SceneViewsLinks : MonoBehaviour
    {
        [SerializeField] private List<BasicView> _views;

        public List<BasicView> Views { get => _views; }
    }
}
