using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Orders
{
    public class DialogueView : MonoBehaviour
    {
        [SerializeField] private Button _acceptButton;
        [SerializeField] private Button _rejectButton;
        [SerializeField] private TMP_Text _titleText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Image _clientImage;
        [SerializeField] private Button _nextButton;
        [SerializeField] private Button _prevButton;
        [SerializeField] private Button _dialogueStartButton;
        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private GameObject _dialogueIcon;
        [SerializeField] private TMP_Text _dialogueIconText;

        public Button AcceptButton { get => _acceptButton; }
        public Button RejectButton { get => _rejectButton; }
        public TMP_Text TitleText { get => _titleText; }
        public TMP_Text DescriptionText { get => _descriptionText; }
        public Image ClientImage { get => _clientImage; }
        public Button NextButton { get => _nextButton; }
        public Button PrevButton { get => _prevButton; }
        public Button DialogueStartButton { get => _dialogueStartButton; }
        public GameObject DialoguePanel { get => _dialoguePanel; }
        public GameObject DialogueIcon { get => _dialogueIcon; }
        public TMP_Text DialogueIconText { get => _dialogueIconText; }
    }
}

