using MAEngine;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameCoreModule
{
    public class GameControlActions : IAction, IInitialisation, ICleanUp
    {
        private StateEventsBus _stateEventsBus;
        private CanvasList _canvasList;
        private List<GameObject> _screensList;

        [Inject]
        public void Construct(StateEventsBus stateEventsBus, CanvasList canvasList)
        {
            _stateEventsBus = stateEventsBus;
            _canvasList = canvasList;
        }

        public void Initialisation()
        {
            _stateEventsBus.OnDialogueStateActivate += ShowDialogueScreen;
            _stateEventsBus.OnOrdersStateActivate += ShowOrdersScreen;
            _stateEventsBus.OnShopStateActivate += ShowShopScreen;
            _stateEventsBus.OnMaterialsStateActivate += ShowMaterialsScreen;
            _stateEventsBus.OnForgingStateActivate += ShowForgingScreen;
            _stateEventsBus.OnResultsStateActivate += ShowResultsScreen;
            _stateEventsBus.OnTradeStateActivate += ShowTradeScreen;
            InitializeScreensList();
        }

        public void Cleanup()
        {
            _stateEventsBus.OnDialogueStateActivate -= ShowDialogueScreen;
            _stateEventsBus.OnOrdersStateActivate -= ShowOrdersScreen;
            _stateEventsBus.OnShopStateActivate -= ShowShopScreen;
            _stateEventsBus.OnMaterialsStateActivate -= ShowMaterialsScreen;
            _stateEventsBus.OnForgingStateActivate -= ShowForgingScreen;
            _stateEventsBus.OnResultsStateActivate -= ShowResultsScreen;
            _stateEventsBus.OnTradeStateActivate -= ShowTradeScreen;
        }

        private void InitializeScreensList()
        {
            _screensList = new List<GameObject>
            {
                _canvasList.DialogueCanvas,
                _canvasList.OrdersCanvas,
                _canvasList.ShopCanvas,
                _canvasList.MaterialsCanvas,
                _canvasList.ForgingCanvas,
                _canvasList.ResultsCanvas,
                _canvasList.TradeCanvas
            };

        }

        private void ShowDialogueScreen()
        {
            ShowCurrentScreen(_canvasList.DialogueCanvas);
        }

        private void ShowOrdersScreen()
        {
            ShowCurrentScreen(_canvasList.OrdersCanvas);
        }

        private void ShowShopScreen()
        {
            ShowCurrentScreen(_canvasList.ShopCanvas);
        }

        private void ShowMaterialsScreen()
        {
            ShowCurrentScreen(_canvasList.MaterialsCanvas);
        }

        private void ShowForgingScreen()
        {
            ShowCurrentScreen(_canvasList.ForgingCanvas);
        }

        private void ShowResultsScreen()
        {
            ShowCurrentScreen(_canvasList.ResultsCanvas);
        }
        
        
        private void ShowTradeScreen()
        {
            ShowCurrentScreen(_canvasList.TradeCanvas);
        }

        private void ShowCurrentScreen(GameObject targetScreenObject)
        {
            foreach (GameObject canvasObject in _screensList)
            {
                if (canvasObject == targetScreenObject)
                {
                    canvasObject.SetActive(true);
                }
                else
                {
                    canvasObject.SetActive(false);
                }
            }
        }
    }
}
