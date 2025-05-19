using TMPro;
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
        [SerializeField] private TMP_Text _currentMoneyText;
        [SerializeField] private UpgradeConfirmView _upgradeConfirmView;
        [SerializeField] private Button _clearProgressButton;
        [SerializeField] private GameObject _orderEndedPanel;
        [SerializeField] private GameObject _orderTimePanel;
        [SerializeField] private Slider _orderTimeSlider;
        [SerializeField] private GameObject _uiBlockingPanel;
        [SerializeField] private Button _addMoneyButton;

        public Button DialogueButton { get => _dialogueButton; }
        public Button OrdersButton { get => _ordersButton; }
        public Button ShopButton { get => _shopButton; }
        public Button MaterialsButton { get => _materialsButton; }
        public RectTransform ClientsQueueTransform { get => _clientsQueueTransform; }
        public GameObject NavigationPanel { get => _navigationPanel; }
        public TMP_Text CurrentMoneyText { get => _currentMoneyText; }
        public UpgradeConfirmView UpgradeConfirmView { get => _upgradeConfirmView; }
        public Button ClearProgressButton { get => _clearProgressButton; }
        public GameObject OrderEndedPanel { get => _orderEndedPanel; }
        public GameObject OrderTimePanel { get => _orderTimePanel; }
        public Slider OrderTimeSlider { get => _orderTimeSlider; }
        public GameObject UIBlockingPanel { get => _uiBlockingPanel; }
        public Button AddMoneyButton { get => _addMoneyButton; }
        
    }
}

