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

        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private RectTransform _clientsQueueTransform;

        public Button AcceptButton { get => _acceptButton; }
        public Button RejectButton { get => _rejectButton; }
        public TMP_Text TitleText { get => _titleText; }
        public TMP_Text DescriptionText { get => _descriptionText; }
        public Image ClientImage { get => _clientImage; }
        public GameObject DialoguePanel { get => _dialoguePanel; }
        public RectTransform ClientsQueueTransform { get => _clientsQueueTransform; }
    }
}

