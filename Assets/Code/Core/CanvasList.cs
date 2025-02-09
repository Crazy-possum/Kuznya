using UnityEngine;

namespace GameCoreModule
{
    public class CanvasList : MonoBehaviour
    {
        [SerializeField] private GameObject _forgingCanvas;
        [SerializeField] private GameObject _resultsCanvas;

        public GameObject ForgingCanvas { get => _forgingCanvas; }
        public GameObject ResultsCanvas { get => _resultsCanvas; }
    }
}
