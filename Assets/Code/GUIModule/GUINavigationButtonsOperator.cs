using GameCoreModule;
using MAEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Zenject;

namespace MainGUI
{
    public class GUINavigationButtonsOperator : IAction, IInitialisation, ICleanUp
    {
        private GUIView _view;
        private StateEventsBus _events;
        private List<Button> _guiButtons;

        [Inject]
        public void Construct(GUIView view, StateEventsBus events)
        {
            _view = view;
            _events = events;
        }

        public void Initialisation()
        {
            _view.DialogueButton.onClick.AddListener(() => ShowDialogue());
            _view.OrdersButton.onClick.AddListener(() => ShowOrders());
            _view.ShopButton.onClick.AddListener(() => ShowShop());
            _view.MaterialsButton.onClick.AddListener(() => ShowMaterials());
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
            _view.OrdersButton.onClick.RemoveListener(() => ShowOrders());
            _view.ShopButton.onClick.RemoveListener(() => ShowShop());
            _view.MaterialsButton.onClick.RemoveListener(() => ShowMaterials());
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

    }
}

