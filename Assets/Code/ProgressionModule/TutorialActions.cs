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
            AskAboutTutorial();
            if (!_playerMetaData.TutorialInfo.IsGameStarted)
            {
                _playerMetaData.TutorialInfo.IsGameStarted = true;
                
            }

            if (_playerMetaData.TutorialInfo.IsTutorialStarted)
            {
                //SubscribeTutorialEvents();
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
            
            if (!_playerMetaData.TutorialInfo.IsForgingAfterSmelting)
            {
                _tutorialEventBus.OnForgingStarted += StartForgingAfterSmelting;
            }
            
            if (!_playerMetaData.TutorialInfo.IsTradeStarted)
            {
                _tutorialEventBus.OnTradeStarted += TradeStarted;
            }
            
            if (!_playerMetaData.TutorialInfo.IsTradeScreenOpened)
            {
                _tutorialEventBus.OnTradeScreenOpened += TradeScreenOpened;
            }

            if (!_playerMetaData.TutorialInfo.IsTradeGreenZone)
            {
                _tutorialEventBus.OnTradeGreenZone += TradeGreenZone;
            }

            if (!_playerMetaData.TutorialInfo.IsTradeFinished)
            {
                _tutorialEventBus.OnTradeFinished += TradeFinished;
            }

            if (!_playerMetaData.TutorialInfo.IsHardeningStarted)
            {
                _tutorialEventBus.OnHardeningStarted += HardeningStarted;
            }

            if (!_playerMetaData.TutorialInfo.IsHardeningGoodHit)
            {
                _tutorialEventBus.OnHardeningGoodHit += HardeningGoodHit;
            }

            if (!_playerMetaData.TutorialInfo.IsSharpeningStarted)
            {
                _tutorialEventBus.OnSharpeningStarted += SharpeningStarted;
            }

            if (!_playerMetaData.TutorialInfo.IsSharpeningGoodHit)
            {
                _tutorialEventBus.OnSharpeningGoodHit += SharpeningGoodHit;
            }

            if (!_playerMetaData.TutorialInfo.IsSharpeningBadHit)
            {
                _tutorialEventBus.OnSharpeningBadHit += SharpeningBadHit;
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
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void ForgingStarted()
        {
            _tutorialEventBus.OnForgingStarted -= ForgingStarted;
            _playerMetaData.TutorialInfo.IsForgingStarted = true;
            _tutorialView.ForgingStartedTutorialPanel.SetActive(true);
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void ForgingHit()
        {
            _tutorialView.ForgingStartedTutorialPanel.SetActive(false);
            _tutorialEventBus.OnForgingHit -= ForgingHit;
            _playerMetaData.TutorialInfo.IsForgingHit = true;
            _tutorialView.ForgingFirstHitTutorialPanel.SetActive(true);
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void ForgingStageChanged()
        {
            _tutorialEventBus.OnForgingStageChanged -= ForgingStageChanged;
            _playerMetaData.TutorialInfo.IsForgingStageChanged = true;
            _tutorialView.ForgingChangeStageTutorialPanel.SetActive(true);
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void ResultScreenOpened()
        {
            _tutorialEventBus.OnResultScreenOpened -= ResultScreenOpened;
            _playerMetaData.TutorialInfo.IsResultScreenOpened = true;
            _tutorialView.ResultScreenTutorialPanel.SetActive(true);
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void OrderFinished()
        {
            _tutorialView.ResultScreenTutorialPanel.SetActive(false);
            _tutorialEventBus.OnOrderFinished -= OrderFinished;
            _playerMetaData.TutorialInfo.IsOrderFinished = true;
            _tutorialView.OrderFinishedTutorialPanel.SetActive(true);
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void OrderSubmitStarted()
        {
            _tutorialEventBus.OnOrderSubmitStarted -= OrderSubmitStarted;
            _playerMetaData.TutorialInfo.IsOrderSubmitStarted = true;
            _tutorialView.OrderSubmitOrderTutorialPanel.SetActive(true);
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void MoneyAdded()
        {
            _tutorialView.OrderSubmitOrderTutorialPanel.SetActive(false);
            _tutorialEventBus.OnMoneyAdded -= MoneyAdded;
            _playerMetaData.TutorialInfo.IsMoneyAdded = true;
            _tutorialView.FristMoneyTutorialPanel.SetActive(true);
            SetBlocker(_tutorialView.Blocker3);
            _tutorialEventBus.OnOrderSectionFinished?.Invoke();
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
            _tutorialEventBus.OnAdditionalStagesBought -= AdditionalStagesBought;
            _playerMetaData.TutorialInfo.IsAdditionalStagesBought = true;
            _tutorialView.AdditionalStageAddedTutorialPanel.SetActive(true);
            SetBlocker(_tutorialView.Blocker5);
        }

        private void SmeltingStarted()
        {
            Time.timeScale = 0;
            _tutorialEventBus.OnSmeltingStarted -= SmeltingStarted;
            _playerMetaData.TutorialInfo.IsSmeltingStarted = true;
            _tutorialView.SmeltingStartedTutorialPanel.SetActive(true);
            _tutorialView.SmeltingContinueButton.Button.onClick.AddListener(SmeltingContinue);
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void SmeltingContinue()
        {
            Time.timeScale = 1;
            _tutorialView.SmeltingStartedTutorialPanel.SetActive(false);
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void SmeltingNearTrigger()
        {
            Time.timeScale = 0;
            _tutorialEventBus.OnSmeltingNearTrigger -= SmeltingNearTrigger;
            _playerMetaData.TutorialInfo.IsSmeltingNearTrigger = true;
            _tutorialView.SmeltingNearTriggerTutorialPanel.SetActive(true);
            _tutorialEventBus.OnSmeltingNearTriggerHit += ContinueSmelting;
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void ContinueSmelting()
        {
            Time.timeScale = 1;
            _tutorialEventBus.OnSmeltingNearTriggerHit -= ContinueSmelting;
            _tutorialView.SmeltingNearTriggerTutorialPanel.SetActive(false);
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void SmeltingGoodHit()
        {
            Time.timeScale = 0;
            _tutorialEventBus.OnSmeltingGoodHit -= SmeltingGoodHit;
            _playerMetaData.TutorialInfo.IsSmeltingGoodHit = true;
            _tutorialView.SmeltingGoodHitTutorialPanel.SetActive(true);
            _tutorialView.SmeltingContinueButton2.Button.onClick.AddListener(ContinueSmelting2);
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }
        
        private void ContinueSmelting2()
        {
            Time.timeScale = 1;
            _tutorialView.SmeltingGoodHitTutorialPanel.SetActive(false);
            _tutorialView.SmeltingContinueButton2.Button.onClick.RemoveAllListeners();
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }
        
        private void StartForgingAfterSmelting()
        {
            if (_playerMetaData.TutorialInfo.IsSmeltingGoodHit)
            {
                _tutorialEventBus.OnForgingStarted -= StartForgingAfterSmelting;
                _playerMetaData.TutorialInfo.IsForgingAfterSmelting = true;
                _tutorialView.ForgingAfterSmeltingTutorialPanel.SetActive(true);
                _tutorialEventBus.OnOrderSectionFinished?.Invoke();
            }
        }

        private void TradeStarted()
        {
            _tutorialEventBus.OnTradeStarted -= TradeStarted;
            _playerMetaData.TutorialInfo.IsTradeStarted = true;
            _tutorialView.TradeStartTutorialPanel.SetActive(true);
        }
        
        private void TradeScreenOpened()
        {
            Time.timeScale = 0;
            _tutorialEventBus.OnTradeScreenOpened -= TradeScreenOpened;
            _playerMetaData.TutorialInfo.IsTradeScreenOpened = true;
            _tutorialView.TradeStartTutorialPanel.SetActive(false);
            _tutorialView.TradeScreenTutorialPanel.SetActive(true);
            _tutorialView.TradeScreenContinueButton.Button.onClick.AddListener(ContinueTrade);
        }

        private void ContinueTrade()
        {
            _tutorialView.TradeScreenTutorialPanel.SetActive(false);
            _tutorialView.TradeGreenZoneTutorialPanel.SetActive(false);
            _tutorialView.TradeScreenContinueButton.Button.onClick.RemoveAllListeners();
            _tutorialView.TradeScreenContinueButton2.Button.onClick.RemoveAllListeners();
            Time.timeScale = 1;
        }

        private void TradeGreenZone()
        {
            Time.timeScale = 0;
            _tutorialEventBus.OnTradeGreenZone -= TradeGreenZone;
            _playerMetaData.TutorialInfo.IsTradeGreenZone = true;
            _tutorialView.TradeGreenZoneTutorialPanel.SetActive(true);
            _tutorialView.TradeScreenContinueButton2.Button.onClick.AddListener(ContinueTrade);
        }

        private void TradeFinished()
        {
            _tutorialEventBus.OnTradeFinished -= TradeFinished;
            _playerMetaData.TutorialInfo.IsTradeFinished = true;
            _tutorialView.TradeFinishTutorialPanel.SetActive(true);
        }

        private void HardeningStarted()
        {
            Time.timeScale = 0;
            _tutorialEventBus.OnHardeningStarted -= HardeningStarted;
            _playerMetaData.TutorialInfo.IsHardeningStarted = true;
            _tutorialView.HardeningStartTutorialPanel.SetActive(true);
            _tutorialView.HardeningContinueButton.Button.onClick.AddListener(HardeningContinue);
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void HardeningContinue()
        {
            Time.timeScale = 1;
            _tutorialView.HardeningStartTutorialPanel.SetActive(false);
            _tutorialView.HardeningGoodHitTutorialPanel.SetActive(false);
            _tutorialView.HardeningContinueButton.Button.onClick.RemoveAllListeners();
            _tutorialView.HardeningContinueButton2.Button.onClick.RemoveAllListeners();
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void HardeningGoodHit()
        {
            Time.timeScale = 0;
            _tutorialEventBus.OnHardeningGoodHit -= HardeningGoodHit;
            _playerMetaData.TutorialInfo.IsHardeningGoodHit = true;
            _tutorialView.HardeningGoodHitTutorialPanel.SetActive(true);
            _tutorialView.HardeningContinueButton2.Button.onClick.AddListener(HardeningContinue);
            _tutorialEventBus.OnOrderSectionFinished?.Invoke();
        }

        private void SharpeningStarted()
        {
            Time.timeScale = 0;
            _tutorialEventBus.OnSharpeningStarted -= SharpeningStarted;
            _playerMetaData.TutorialInfo.IsSharpeningStarted = true;
            _tutorialView.SharpeningStartTutorialPanel.SetActive(true);
            _tutorialView.SharpeningContinueButton.Button.onClick.AddListener(SharpeningContinue);
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void SharpeningContinue()
        {
            Time.timeScale = 1;
            _tutorialView.SharpeningStartTutorialPanel.SetActive(false);
            _tutorialView.SharpeningGoodHitTutorialPanel.SetActive(false);
            _tutorialView.SharpeningBadHitTutorialPanel.SetActive(false);
            _tutorialView.SharpeningContinueButton.Button.onClick.RemoveAllListeners();
            _tutorialView.SharpeningContinue2Button.Button.onClick.RemoveAllListeners();
            _tutorialView.SharpeningContinue3Button.Button.onClick.RemoveAllListeners();
            _tutorialEventBus.OnOrderSectionStarted?.Invoke();
        }

        private void SharpeningGoodHit()
        {
            Time.timeScale = 0;
            _tutorialEventBus.OnSharpeningGoodHit -= SharpeningGoodHit;
            _tutorialEventBus.OnSharpeningBadHit -= SharpeningBadHit;
            _playerMetaData.TutorialInfo.IsSharpeningGoodHit = true;
            _playerMetaData.TutorialInfo.IsSharpeningBadHit = true;
            _tutorialView.SharpeningGoodHitTutorialPanel.SetActive(true);
            _tutorialView.SharpeningContinue2Button.Button.onClick.AddListener(SharpeningContinue);
            _tutorialEventBus.OnOrderSectionFinished?.Invoke();
        }

        private void SharpeningBadHit()
        {
            Time.timeScale = 0;
            _tutorialEventBus.OnSharpeningGoodHit -= SharpeningGoodHit;
            _tutorialEventBus.OnSharpeningBadHit -= SharpeningBadHit;
            _playerMetaData.TutorialInfo.IsSharpeningBadHit = true;
            _playerMetaData.TutorialInfo.IsSharpeningGoodHit = true;
            _tutorialView.SharpeningBadHitTutorialPanel.SetActive(true);
            _tutorialView.SharpeningContinue3Button.Button.onClick.AddListener(SharpeningContinue);
            _tutorialEventBus.OnOrderSectionFinished?.Invoke();
        }
        

        public void Cleanup()
        {

        }
    }
}