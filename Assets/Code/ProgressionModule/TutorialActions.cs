using GameCoreModule;
using MAEngine;
using Orders;
using UnityEngine;

namespace Progression
{
    public class TutorialActions : IAction, IInitialisation, ICleanUp
    {
        private ProgressionData _data;
        private TutorialEventBus _tutorialEventBus;
        private TutorialView _tutorialView;

        private PlayerMetaData _playerMetaData;

        public TutorialActions(ProgressionData data, TutorialEventBus tutorialEventBus, TutorialView tutorialView)
        {
            _data = data;
            _tutorialEventBus = tutorialEventBus;
            _tutorialView = tutorialView;
        }

        public void Initialisation()
        {
            _playerMetaData = _data.PlayerMetaData;
            if (!_playerMetaData.TutorialInfo.IsGameStarted)
            {
                _playerMetaData.TutorialInfo.IsGameStarted = true;
                AskAboutTutorial();
            }

            if (_playerMetaData.TutorialInfo.IsTutorialStarted)
            {
                SubscribeTutorialEvents();
            }
        }

        private void AskAboutTutorial()
        {
            Time.timeScale = 0;
            _tutorialView.TutorialAskPanel.SetActive(true);
            _tutorialView.TutorialStartButton.Button.onClick.AddListener(StartTutorial);
            _tutorialView.TutorialSkipButton.Button.onClick.AddListener(SkipTutorial);
        }

        private void StartTutorial()
        {
            _tutorialView.TutorialAskPanel.SetActive(false);
            _playerMetaData.TutorialInfo.IsTutorialStarted = true;
            _tutorialEventBus.OnTutorialStarted?.Invoke();
            SubscribeTutorialEvents();
            _tutorialView.TutorialStartedPanel.SetActive(true);
            _tutorialView.TutorialContinueButton.Button.onClick.AddListener(ContinueTutorial);
        }

        private void ContinueTutorial()
        {
            Time.timeScale = 1;
            _tutorialView.TutorialStartedPanel.SetActive(false);
            _tutorialView.TutorialContinueButton.Button.onClick.RemoveAllListeners();
        }

        private void SkipTutorial()
        {
            Time.timeScale = 1;
            _tutorialView.TutorialAskPanel.SetActive(false);
            _playerMetaData.TutorialInfo.IsTutorialSkiped = true;
            _tutorialView.gameObject.SetActive(false);
            _tutorialEventBus.OnTutorialSkiped?.Invoke();
        }
        
        private void SubscribeTutorialEvents()
        {
            if (!_playerMetaData.TutorialInfo.IsClientActivated)
            {
                _tutorialEventBus.OnClientActivated += ClientActivated;
            }

            if (!_playerMetaData.TutorialInfo.IsDialogueStarted)
            {
                _tutorialEventBus.OnDialogueStarted += DialogueStarted;
            }

            if (!_playerMetaData.TutorialInfo.IsOrderApplied)
            {
                _tutorialEventBus.OnOrderApplied += OrderApplied;
            }

            if (!_playerMetaData.TutorialInfo.IsOrdersScreenOpened)
            {
                _tutorialEventBus.OnOrdersScreenOpened += OrdersScreenOpened;
            }

            if (!_playerMetaData.TutorialInfo.IsForgingStarted)
            {
                _tutorialEventBus.OnForgingStarted += ForgingStarted;
            }

            if (!_playerMetaData.TutorialInfo.IsForgingHit)
            {
                _tutorialEventBus.OnForgingHit += ForgingHit;
            }

            if (!_playerMetaData.TutorialInfo.IsForgingStageChanged)
            {
                _tutorialEventBus.OnForgingStageChanged += ForgingStageChanged;
            }

            if (!_playerMetaData.TutorialInfo.IsResultScreenOpened)
            {
                _tutorialEventBus.OnResultScreenOpened += ResultScreenOpened;
            }

            if (!_playerMetaData.TutorialInfo.IsOrderFinished)
            {
                _tutorialEventBus.OnOrderFinished += OrderFinished;
            }

            if (!_playerMetaData.TutorialInfo.IsOrderSubmitStarted)
            {
                _tutorialEventBus.OnOrderSubmitStarted += OrderSubmitStarted;
            }

            if (!_playerMetaData.TutorialInfo.IsMoneyAdded)
            {
                _tutorialEventBus.OnMoneyAdded += MoneyAdded;
            }

            if (!_playerMetaData.TutorialInfo.IsShopScreenOpened)
            {
                _tutorialEventBus.OnShopScreenOpened += ShopScreenOpened;
            }

            if (!_playerMetaData.TutorialInfo.IsMoneyAccumulated)
            {
                _tutorialEventBus.OnMoneyAccumulated += MoneyAccumulated;
            }

            if (!_playerMetaData.TutorialInfo.IsUpgradeBought)
            {
                _tutorialEventBus.OnUpgradeBought += UpgradeBought;
            }

            if (!_playerMetaData.TutorialInfo.IsOrderMaterialUnavailable)
            {
                _tutorialEventBus.OnOrderMaterialUnavaliable += OrderMaterialUnavailable;
            }

            if (!_playerMetaData.TutorialInfo.IsMaterialScreenOpened)
            {
                _tutorialEventBus.OnMaterialScreenOpened += MaterialScreenOpened;
            }

            if (!_playerMetaData.TutorialInfo.IsMaterialChanged)
            {
                _tutorialEventBus.OnMaterialChanged += MaterialChanged;
            }

            if (!_playerMetaData.TutorialInfo.IsOrdersMaterialOpened)
            {
                _tutorialEventBus.OnOrdersMaterialOpened += OrdersMaterialOpened;
            }

            if (!_playerMetaData.TutorialInfo.IsAdditionalStagesBought)
            {
                _tutorialEventBus.OnAdditionalStagesBought += AdditionalStagesBought;
            }

            if (!_playerMetaData.TutorialInfo.IsSmeltingStarted)
            {
                _tutorialEventBus.OnSmeltingStarted += SmeltingStarted;
            }

            if (!_playerMetaData.TutorialInfo.IsSmeltingNearTrigger)
            {
                _tutorialEventBus.OnSmeltingNearTrigger += SmeltingNearTrigger;
            }

            if (!_playerMetaData.TutorialInfo.IsSmeltingGoodHit)
            {
                _tutorialEventBus.OnSmeltingGoodHit += SmeltingGoodHit;
            }
        }
        
        private void ClientActivated()
        {
            Time.timeScale = 0;
            _playerMetaData.TutorialInfo.IsClientActivated = true;
            _tutorialEventBus.OnClientActivated -= ClientActivated;
            _tutorialView.ClientTutorialPanel.SetActive(true);
            _tutorialEventBus.OnDialogueStarted += ClientContinue;

        }
        
        private void ClientContinue(ActiveOrder order)
        {
            _tutorialEventBus.OnDialogueStarted -= ClientContinue;
            Time.timeScale = 1;
            _tutorialView.ClientTutorialPanel.SetActive(false);
        }
        
        private void DialogueStarted(ActiveOrder order)
        {
            _tutorialView.DialogueTutorialPanel.SetActive(true);
            _tutorialEventBus.OnDialogueStarted -= DialogueStarted;
            _tutorialEventBus.OnOrderApplied += DialogueContinue;
        }

        private void DialogueContinue()
        {
            _tutorialView.DialogueTutorialPanel.SetActive(false);
            _tutorialEventBus.OnOrderApplied -= DialogueContinue;
        }
        
        private void OrderApplied()
        {
            _tutorialEventBus.OnOrderApplied -= OrderApplied;
            _playerMetaData.TutorialInfo.IsOrderApplied = true;
        }

        private void OrdersScreenOpened()
        {
            throw new System.NotImplementedException();
        }

        private void ForgingStarted()
        {
            throw new System.NotImplementedException();
        }

        private void ForgingHit()
        {
            throw new System.NotImplementedException();
        }

        private void ForgingStageChanged()
        {
            throw new System.NotImplementedException();
        }

        private void ResultScreenOpened()
        {
            throw new System.NotImplementedException();
        }

        private void OrderFinished()
        {
            throw new System.NotImplementedException();
        }

        private void OrderSubmitStarted()
        {
            throw new System.NotImplementedException();
        }

        private void MoneyAdded()
        {
            throw new System.NotImplementedException();
        }

        private void ShopScreenOpened()
        {
            throw new System.NotImplementedException();
        }

        private void MoneyAccumulated()
        {
            throw new System.NotImplementedException();
        }

        private void UpgradeBought()
        {
            throw new System.NotImplementedException();
        }

        private void OrderMaterialUnavailable()
        {
            throw new System.NotImplementedException();
        }

        private void MaterialScreenOpened()
        {
            throw new System.NotImplementedException();
        }

        private void MaterialChanged()
        {
            throw new System.NotImplementedException();
        }

        private void OrdersMaterialOpened()
        {
            throw new System.NotImplementedException();
        }

        private void AdditionalStagesBought()
        {
            throw new System.NotImplementedException();
        }

        private void SmeltingStarted()
        {
            throw new System.NotImplementedException();
        }

        private void SmeltingNearTrigger()
        {
            throw new System.NotImplementedException();
        }

        private void SmeltingGoodHit()
        {
            throw new System.NotImplementedException();
        }
        

        public void Cleanup()
        {

        }
    }
}