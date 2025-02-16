using Progression;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace MainGUI
{
    public class GUIView : MonoBehaviour
    {
        [SerializeField] private Button _dialogueButton;
        [SerializeField] private Button _ordersButton;
        [SerializeField] private Button _shopButton;
        [SerializeField] private Button _materialsButton;
        [SerializeField] private RectTransform _clientsQueueTransform;
        [SerializeField] private GameObject _navigationPanel;

        public Button DialogueButton { get => _dialogueButton; }
        public Button OrdersButton { get => _ordersButton; }
        public Button ShopButton { get => _shopButton; }
        public Button MaterialsButton { get => _materialsButton; }
        public RectTransform ClientsQueueTransform { get => _clientsQueueTransform; }
        public GameObject NavigationPanel { get => _navigationPanel; }
    }
}

