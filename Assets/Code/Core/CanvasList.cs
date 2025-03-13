using UnityEngine;

namespace GameCoreModule
{
    public class CanvasList : MonoBehaviour
    {
        [SerializeField] private GameObject _dialogueCanvas;
        [SerializeField] private GameObject _ordersCanvas;
        [SerializeField] private GameObject _shopCanvas;
        [SerializeField] private GameObject _materialsCanvas;
        [SerializeField] private GameObject _forgingCanvas;
        [SerializeField] private GameObject _resultsCanvas;
        [SerializeField] private GameObject _tradeCanvas;

        public GameObject DialogueCanvas { get => _dialogueCanvas; }
        public GameObject OrdersCanvas { get => _ordersCanvas; }
        public GameObject ShopCanvas { get => _shopCanvas; }
        public GameObject MaterialsCanvas { get => _materialsCanvas; }
        public GameObject ForgingCanvas { get => _forgingCanvas; }
        public GameObject ResultsCanvas { get => _resultsCanvas; }
        public GameObject TradeCanvas { get => _tradeCanvas; }

    }
}
