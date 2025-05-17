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
        private GameObject _activeBlocker;

        public TutorialActions(ProgressionData data, TutorialEventBus tutorialEventBus, TutorialView tutorialView)
        {
            _data = data;
            _tutorialEventBus = tutorialEventBus;
            _tutorialView = tutorialView;
        }

        public void Initialisation()
        {
            _playerMetaData = _data.PlayerMetaData;
            _activeBlocker = _tutorialView.Blocker1;
            _activeBlocker.SetActive(true);
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
            _tutorialEventBus.OnTutorialStartConfirmed?.Invoke();
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
        
        private void SetBlocker(GameObject blocker)
        {
            _activeBlocker.SetActive(false);
            _activeBlocker = blocker;
            _activeBlocker.SetActive(true);
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
            _tutorialView.TutorialStartedPanel.SetActive(false);
            _tutorialView.TutorialContinueButton.Button.onClick.RemoveAllListeners();
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
            _tutorialView.OrderAcceptedTutorialPanel.SetActive(true);
            SetBlocker(_tutorialView.Blocker2);
        }

        private void OrdersScreenOpened()
        {
            _tutorialView.OrderAcceptedTutorialPanel.SetActive(false);
            _tutorialEventBus.OnOrdersScreenOpened -= OrdersScreenOpened;
            _playerMetaData.TutorialInfo.IsOrdersScreenOpened = true;
            _tutorialView.OrdersListTutorialPanel.SetActive(true);
            _tutorialEventBus.OnOrderSelected += OrderStarted;
        }

        private void OrderStarted()
        {
            _tutorialEventBus.OnOrderSelected -= OrderStarted;
            _tutorialView.OrdersListTutorialPanel.SetActive(false);
        }

        private void ForgingStarted()
        {
            _tutorialEventBus.OnForgingStarted -= ForgingStarted;
            _playerMetaData.TutorialInfo.IsForgingStarted = true;
            _tutorialView.ForgingStartedTutorialPanel.SetActive(true);
        }

        private void ForgingHit()
        {
            _tutorialView.ForgingStartedTutorialPanel.SetActive(false);
            _tutorialEventBus.OnForgingHit -= ForgingHit;
            _playerMetaData.TutorialInfo.IsForgingHit = true;
            _tutorialView.ForgingFirstHitTutorialPanel.SetActive(true);
        }

        private void ForgingStageChanged()
        {
            _tutorialEventBus.OnForgingStageChanged -= ForgingStageChanged;
            _playerMetaData.TutorialInfo.IsForgingStageChanged = true;
            _tutorialView.ForgingChangeStageTutorialPanel.SetActive(true);
        }

        private void ResultScreenOpened()
        {
            _tutorialEventBus.OnResultScreenOpened -= ResultScreenOpened;
            _playerMetaData.TutorialInfo.IsResultScreenOpened = true;
            _tutorialView.ResultScreenTutorialPanel.SetActive(true);
        }

        private void OrderFinished()
        {
            _tutorialView.ResultScreenTutorialPanel.SetActive(false);
            _tutorialEventBus.OnOrderFinished -= OrderFinished;
            _playerMetaData.TutorialInfo.IsOrderFinished = true;
            _tutorialView.OrderFinishedTutorialPanel.SetActive(true);
        }

        private void OrderSubmitStarted()
        {
            _tutorialEventBus.OnOrderSubmitStarted -= OrderSubmitStarted;
            _playerMetaData.TutorialInfo.IsOrderSubmitStarted = true;
            _tutorialView.OrderSubmitOrderTutorialPanel.SetActive(true);
        }

        private void MoneyAdded()
        {
            _tutorialView.OrderSubmitOrderTutorialPanel.SetActive(false);
            _tutorialEventBus.OnMoneyAdded -= MoneyAdded;
            _playerMetaData.TutorialInfo.IsMoneyAdded = true;
            _tutorialView.FristMoneyTutorialPanel.SetActive(true);
            SetBlocker(_tutorialView.Blocker3);
        }

        private void ShopScreenOpened()
        {
            _tutorialView.OrderSubmitOrderTutorialPanel.SetActive(false);
            _tutorialView.FirstMoney2TutorialPanel.SetActive(false);
            _tutorialEventBus.OnShopScreenOpened -= ShopScreenOpened;
            _playerMetaData.TutorialInfo.IsShopScreenOpened = true;
            _tutorialView.ShopTutorialPanel.SetActive(true);
            _tutorialView.ShopTutorialContinueButton.Button.onClick.AddListener(StartFreeSection);
        }

        private void StartFreeSection()
        {
            SetBlocker(_tutorialView.Blocker5);
            _tutorialView.ShopTutorialContinueButton.Button.onClick.RemoveAllListeners();
        }

        private void MoneyAccumulated()
        {
            SetBlocker(_tutorialView.Blocker3);
            _tutorialView.MoneyAccumulatedTutorialPanel.SetActive(true);
            _tutorialEventBus.OnMoneyAccumulated -= MoneyAccumulated;
            _playerMetaData.TutorialInfo.IsMoneyAccumulated = true;
        }

        private void UpgradeBought()
        {
            _tutorialEventBus.OnUpgradeBought -= UpgradeBought;
            _playerMetaData.TutorialInfo.IsUpgradeBought = true;
            _tutorialView.UpgradeBoughtTutorialPanel.SetActive(true);
            SetBlocker(_tutorialView.Blocker5);
        }

        private void OrderMaterialUnavailable()
        {
            _tutorialEventBus.OnOrderMaterialUnavaliable -= OrderMaterialUnavailable;
            _playerMetaData.TutorialInfo.IsOrderMaterialUnavailable = true;
            _tutorialView.MaterialUnavaliableTutorialPanel.SetActive(true);
            SetBlocker(_tutorialView.Blocker4);
        }

        private void MaterialScreenOpened()
        {
            if (_playerMetaData.TutorialInfo.IsOrderMaterialUnavailable)
            {
                _tutorialEventBus.OnMaterialScreenOpened -= MaterialScreenOpened;
                _playerMetaData.TutorialInfo.IsMaterialScreenOpened = true;
                _tutorialView.MaterialUnavaliableTutorialPanel.SetActive(false);
                _tutorialView.MaterialsTutorialPanel.SetActive(true);
            }
        }

        private void MaterialChanged()
        {
            if (_playerMetaData.TutorialInfo.IsOrderMaterialUnavailable)
            {
                _tutorialEventBus.OnMaterialChanged -= MaterialChanged;
                _playerMetaData.TutorialInfo.IsMaterialChanged = true;
                _tutorialView.Materials2TutorialPanel.SetActive(false);
                _tutorialView.MaterialChangingTutorialPanel.SetActive(true);
                _tutorialEventBus.OnOrdersScreenOpened += ShowOrderInfoTutorial;
                SetBlocker(_tutorialView.Blocker2);
            }
        }
        
        private void ShowOrderInfoTutorial()
        {
            _tutorialEventBus.OnOrdersScreenOpened -= ShowOrderInfoTutorial;
            _tutorialView.MaterialChangingTutorialPanel.SetActive(false);
            _tutorialView.OrderInfoTutorialPanel.SetActive(true);
        }

        private void OrdersMaterialOpened()
        {
            if (_playerMetaData.TutorialInfo.IsMaterialChanged)
            {
                _tutorialEventBus.OnOrdersMaterialOpened -= OrdersMaterialOpened;
                _playerMetaData.TutorialInfo.IsOrdersMaterialOpened = true;
                _tutorialView.OrderInfoTutorialPanel.SetActive(false);
                _tutorialView.OrderMaterialsTutorialPanel.SetActive(true);
                _tutorialView.BasicTutorialEndButton.Button.onClick.AddListener(EndBasicTutorial);
            }
        }

        private void EndBasicTutorial()
        {
            SetBlocker(_tutorialView.Blocker5);
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