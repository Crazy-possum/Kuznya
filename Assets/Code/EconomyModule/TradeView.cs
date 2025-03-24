using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Economy
{
    public class TradeView : MonoBehaviour
    {
        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private GameObject _tradePanel;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _tradeButton;
        [SerializeField] private Button _tradeClickerButton;
        [SerializeField] private Slider _tradeSlider;
        [SerializeField] private TMP_Text _timer;
        [SerializeField] private TMP_Text _multipler;
        [SerializeField] private TMP_Text _rewardText;
        [SerializeField] private Image _clientImage;

        public GameObject DialoguePanel => _dialoguePanel;
        public GameObject TradePanel => _tradePanel;
        public Button ConfirmButton => _confirmButton;
        public Button TradeButton => _tradeButton;
        public Button TradeClickerButton => _tradeClickerButton;
        public Slider TradeSlider => _tradeSlider;
        public TMP_Text Timer => _timer;
        public TMP_Text Multipler => _multipler;
        public TMP_Text RewardText => _rewardText;
        public Image ClientImage => _clientImage;
        
        public void ShowTradePanel()
        {
            _tradePanel.gameObject.SetActive(true);
            _dialoguePanel.gameObject.SetActive(false);
        }
        
        public void HideTradePanel()
        {
            _tradePanel.gameObject.SetActive(false);
            _dialoguePanel.gameObject.SetActive(true);
        }
    }
}