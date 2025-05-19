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
        [SerializeField] private TMP_Text _clientName;
        [SerializeField] private TMP_Text _additionRewardText;
        [SerializeField] private TMP_Text _fullRewardText;
        [SerializeField] private Color _goodColor;
        [SerializeField] private Color _badColor;

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
        public TMP_Text ClientName => _clientName;
        public TMP_Text AdditionRewardText => _additionRewardText;
        public TMP_Text FullRewardText => _fullRewardText;
        public Color GoodColor => _goodColor;
        public Color BadColor => _badColor;
        
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