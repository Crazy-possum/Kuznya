using MainGUI;
using UnityEngine;

namespace Progression
{
    public class TutorialView : MonoBehaviour
    {
        [SerializeField] private GameObject _tutorialAskPanel;
        [SerializeField] private ButtonView _tutorialStartButton;
        [SerializeField] private ButtonView _tutorialSkipButton;
        [SerializeField] private GameObject _tutorialStartedPanel;
        [SerializeField] private ButtonView _tutorialContinueButton;
        [SerializeField] private GameObject _clientTutorialPanel;
        [SerializeField] private GameObject _dialogueTutorialPanel;

        public GameObject TutorialAskPanel => _tutorialAskPanel;
        public ButtonView TutorialStartButton => _tutorialStartButton;
        public ButtonView TutorialSkipButton => _tutorialSkipButton;
        public GameObject TutorialStartedPanel => _tutorialStartedPanel;
        public ButtonView TutorialContinueButton => _tutorialContinueButton;
        public GameObject ClientTutorialPanel => _clientTutorialPanel;
        public GameObject DialogueTutorialPanel => _dialogueTutorialPanel;
    }
}