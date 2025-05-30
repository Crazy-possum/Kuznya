using GameCoreModule;
using MAEngine;
using System.Collections.Generic;
using Orders;
using UnityEngine.UI;
using Zenject;

namespace MainGUI
{
    public class GUINavigationButtonsOperator : IAction, IInitialisation, ICleanUp
    {
        private GUIView _view;
        private StateEventsBus _events;
        private ResultsEventBus _resultsEvents;
        private List<Button> _guiButtons;
        private OrdersEventBus _ordersEventBus;
        private GameEventBus _gameEventBus;
        

        [Inject]
        public void Construct(GUIView view, StateEventsBus events, ResultsEventBus resultsEvents,
            OrdersEventBus ordersEvents, GameEventBus gameEventBus)
        {
            _view = view;
            _events = events;
            _resultsEvents = resultsEvents;
            _ordersEventBus = ordersEvents;
            _gameEventBus = gameEventBus;
        }

        public void Initialisation()
        {
            _view.DialogueButton.onClick.AddListener(() => ShowDialogue());
            _gameEventBus.OnSetDialogueState += ShowDialogue;
            _view.OrdersButton.onClick.AddListener(() => ShowOrders());
            _view.ShopButton.onClick.AddListener(() => ShowShop());
            _view.MaterialsButton.onClick.AddListener(() => ShowMaterials());
            _ordersEventBus.OnOrderStarted += ActivateOrderForging;
            _resultsEvents.OnResultsFinished += StopOrderForging;
            InitializeButtonsList();
            ShowDialogue();
        }

        private void InitializeButtonsList()
        {
            _guiButtons = new List<Button>
            {
                _view.DialogueButton,
                _view.OrdersButton,
                _view.ShopButton,
                _view.MaterialsButton
            };
        }

        public void Cleanup()
        {
            _view.DialogueButton.onClick.RemoveListener(() => ShowDialogue());
            _gameEventBus.OnSetDialogueState -= ShowDialogue;
            _view.OrdersButton.onClick.RemoveListener(() => ShowOrders());
            _view.ShopButton.onClick.RemoveListener(() => ShowShop());
            _view.MaterialsButton.onClick.RemoveListener(() => ShowMaterials());
            _ordersEventBus.OnOrderStarted -= ActivateOrderForging;
            _resultsEvents.OnResultsFinished -= StopOrderForging;
        }

        private void ShowDialogue()
        {
            _events.OnDialogueStateActivate?.Invoke();
            SetCurrentScreenButtonInactive(_view.DialogueButton);
        }
        private void ShowOrders()
        {
            _events.OnOrdersStateActivate?.Invoke();
            SetCurrentScreenButtonInactive(_view.OrdersButton);
        }
        private void ShowShop()
        {
            _events.OnShopStateActivate?.Invoke();
            SetCurrentScreenButtonInactive(_view.ShopButton);
        }
        private void ShowMaterials()
        {
            _events.OnMaterialsStateActivate?.Invoke();
            SetCurrentScreenButtonInactive(_view.MaterialsButton);
        }

        private void SetCurrentScreenButtonInactive(Button currentButton)
        {
            foreach (Button button in _guiButtons)
            {
                if (button == currentButton)
                {
                    button.interactable = false;
                }
                else
                {
                    button.interactable = true;
                }
            }
        }
        
        private void ShowNavigationButtons(int money)
        {
            ShowOrders();
            _view.NavigationPanel.gameObject.SetActive(true);
        }
        
        private void HideNavigationButtons()
        {
            _view.NavigationPanel.gameObject.SetActive(false);
        }

        private void ActivateOrderForging(ActiveOrder activeOrder)
        {
            HideNavigationButtons();
            _view.ClientsQueueTransform.gameObject.SetActive(false);
        }

        private void StopOrderForging(int money)
        {
            ShowNavigationButtons(money);
            _view.ClientsQueueTransform.gameObject.SetActive(true);
        }

    }
}

