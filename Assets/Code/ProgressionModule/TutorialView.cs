using MainGUI;
using UnityEngine;

namespace Progression
{
    public class TutorialView : MonoBehaviour
    {
        [SerializeField] private GameObject _blocker1;
        [SerializeField] private GameObject _blocker2;
        [SerializeField] private GameObject _blocker3;
        [SerializeField] private GameObject _blocker4;
        [SerializeField] private GameObject _blocker5;
        [SerializeField] private GameObject _tutorialAskPanel;
        [SerializeField] private ButtonView _tutorialStartButton;
        [SerializeField] private ButtonView _tutorialSkipButton;
        [SerializeField] private GameObject _tutorialStartedPanel;
        [SerializeField] private ButtonView _tutorialContinueButton;
        [SerializeField] private GameObject _clientTutorialPanel;
        [SerializeField] private GameObject _dialogueTutorialPanel;
        [SerializeField] private GameObject _orderAcceptedTutorialPanel;
        [SerializeField] private GameObject _ordersListTutorialPanel;
        [SerializeField] private GameObject _forgingStartedTutorialPanel;
        [SerializeField] private GameObject _forgingFirstHitTutorialPanel;
        [SerializeField] private GameObject _forgingChangeStageTutorialPanel;
        [SerializeField] private GameObject _resultScreenTutorialPanel;
        [SerializeField] private GameObject _orderFinishedTutorialPanel;
        [SerializeField] private GameObject _orderSubmitOrderTutorialPanel;
        [SerializeField] private GameObject _fristMoneyTutorialPanel;
        [SerializeField] private GameObject _firstMoney2TutorialPanel;
        [SerializeField] private GameObject _shopTutorialPanel;
        [SerializeField] private GameObject _shop2TutorialPanel;
        [SerializeField] private GameObject _shop3TutorialPanel;
        [SerializeField] private ButtonView _shopTutorialContinueButton;
        [SerializeField] private GameObject _moneyAccumulatedTutorialPanel;
        [SerializeField] private GameObject _upradeBoughtTutorialPanel;
        [SerializeField] private GameObject _materialUnavaliableTutorialPanel;
        [SerializeField] private GameObject _materialsTutorialPanel;
        [SerializeField] private GameObject _materials2TutorialPanel;
        [SerializeField] private GameObject _materialChangingTutorialPanel;
        [SerializeField] private GameObject _orderInfoTutorialPanel;
        [SerializeField] private GameObject _orderMaterialsTutorialPanel;
        [SerializeField] private ButtonView _basicTutorialEndButton;
        

        public GameObject Blocker1 => _blocker1;
        public GameObject Blocker2 => _blocker2;
        public GameObject Blocker3 => _blocker3;
        public GameObject Blocker4 => _blocker4;
        public GameObject Blocker5 => _blocker5;
        public GameObject TutorialAskPanel => _tutorialAskPanel;
        public ButtonView TutorialStartButton => _tutorialStartButton;
        public ButtonView TutorialSkipButton => _tutorialSkipButton;
        public GameObject TutorialStartedPanel => _tutorialStartedPanel;
        public ButtonView TutorialContinueButton => _tutorialContinueButton;
        public GameObject ClientTutorialPanel => _clientTutorialPanel;
        public GameObject DialogueTutorialPanel => _dialogueTutorialPanel;
        public GameObject OrderAcceptedTutorialPanel => _orderAcceptedTutorialPanel;
        public GameObject OrdersListTutorialPanel => _ordersListTutorialPanel;
        public GameObject ForgingStartedTutorialPanel => _forgingStartedTutorialPanel;
        public GameObject ForgingFirstHitTutorialPanel => _forgingFirstHitTutorialPanel;
        public GameObject ForgingChangeStageTutorialPanel => _forgingChangeStageTutorialPanel;
        public GameObject ResultScreenTutorialPanel => _resultScreenTutorialPanel;
        public GameObject OrderFinishedTutorialPanel => _orderFinishedTutorialPanel;
        public GameObject OrderSubmitOrderTutorialPanel => _orderSubmitOrderTutorialPanel;
        public GameObject FristMoneyTutorialPanel => _fristMoneyTutorialPanel;
        public GameObject FirstMoney2TutorialPanel => _firstMoney2TutorialPanel;
        public GameObject ShopTutorialPanel => _shopTutorialPanel;
        public GameObject Shop2TutorialPanel => _shop2TutorialPanel;
        public GameObject Shop3TutorialPanel => _shop3TutorialPanel;
        public ButtonView ShopTutorialContinueButton => _shopTutorialContinueButton;
        public GameObject MoneyAccumulatedTutorialPanel => _moneyAccumulatedTutorialPanel;
        public GameObject UpgradeBoughtTutorialPanel => _upradeBoughtTutorialPanel;
        public GameObject MaterialUnavaliableTutorialPanel => _materialUnavaliableTutorialPanel;
        public GameObject MaterialsTutorialPanel => _materialsTutorialPanel;
        public GameObject Materials2TutorialPanel => _materials2TutorialPanel;
        public GameObject MaterialChangingTutorialPanel => _materialChangingTutorialPanel;
        public GameObject OrderInfoTutorialPanel => _orderInfoTutorialPanel;
        public GameObject OrderMaterialsTutorialPanel => _orderMaterialsTutorialPanel;
        public ButtonView BasicTutorialEndButton => _basicTutorialEndButton;
        
    }
}