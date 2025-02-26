using MAEngine;
using UnityEngine;

namespace GameCoreModule
{
    public class BasicView : MonoBehaviour, IView
    {
        [SerializeField] private GameObject _object;
        private string _viewID;

        public GameObject Object { get => _object; }
        public string ViewID { get => _viewID; set => _viewID = value; }
    }
}

