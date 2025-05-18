using System;
using Orders;

namespace GameCoreModule
{
    public class TutorialEventBus
    {
        private Action _onGameStarted;
        private Action _onTutorialStarted;
        private Action _onTutorialSkiped;
        private Action _onTutorialStartConfirmed;
        private Action _onClientActivated;
        private Action<ActiveOrder> _onDialogueStarted;
        private Action _onOrderApplied;
        private Action _onOrdersScreenOpened;
        private Action _onOrderSelected;
        private Action _onForgingStarted;
        private Action _onForgingHit;
        private Action _onForgingStageChanged;
        private Action _onResultScreenOpened;
        private Action _onOrderFinished;
        private Action _onOrderSubmitStarted;
        private Action _onMoneyAdded;
        private Action _onShopScreenOpened;
        private Action _onMoneyAccumulated;
        private Action _onUpgradeBought;
        private Action _onOrderMaterialUnavaliable;
        private Action _onMaterialScreenOpened;
        private Action _onMaterialChanged;
        private Action _onOrdersMaterialOpened;
        private Action _onAdditionalStagesBought;
        private Action _onSmeltingStarted;
        private Action _onSmeltingNearTrigger;
        private Action _onSmeltingNearTriggerHit;
        private Action _onSmeltingGoodHit;
        private Action _onForgingAfterSmelting;
        private Action _onTradeStarted;
        private Action _onTradeScreenOpened;
        private Action _onTradeGreenZone;
        private Action _onTradeFinished;
        private Action _onHardeningStarted;
        private Action _onHardeningGoodHit;
        private Action _onSharpeningStarted;
        private Action _onSharpeningGoodHit;
        private Action _onSharpeningBadHit;
        
        
        public Action OnGameStarted { get => _onGameStarted; set => _onGameStarted = value; }
        public Action OnTutorialStarted { get => _onTutorialStarted; set => _onTutorialStarted = value; }
        public Action OnTutorialSkiped { get => _onTutorialSkiped; set => _onTutorialSkiped = value; }
        public Action OnTutorialStartConfirmed { get => _onTutorialStartConfirmed; set => _onTutorialStartConfirmed = value; }
        public Action OnClientActivated { get => _onClientActivated; set => _onClientActivated = value; }
        public Action<ActiveOrder> OnDialogueStarted { get => _onDialogueStarted; set => _onDialogueStarted = value; }
        public Action OnOrderApplied { get => _onOrderApplied; set => _onOrderApplied = value; }
        public Action OnOrdersScreenOpened { get => _onOrdersScreenOpened; set => _onOrdersScreenOpened = value; }
        public Action OnOrderSelected { get => _onOrderSelected; set => _onOrderSelected = value; }
        public Action OnForgingStarted { get => _onForgingStarted; set => _onForgingStarted = value; }
        public Action OnForgingHit { get => _onForgingHit; set => _onForgingHit = value; }
        public Action OnForgingStageChanged { get => _onForgingStageChanged; set => _onForgingStageChanged = value; }
        public Action OnResultScreenOpened { get => _onResultScreenOpened; set => _onResultScreenOpened = value; }
        public Action OnOrderFinished { get => _onOrderFinished; set => _onOrderFinished = value; }
        public Action OnOrderSubmitStarted { get => _onOrderSubmitStarted; set => _onOrderSubmitStarted = value; }
        public Action OnMoneyAdded { get => _onMoneyAdded; set => _onMoneyAdded = value; }
        public Action OnShopScreenOpened { get => _onShopScreenOpened; set => _onShopScreenOpened = value; }
        public Action OnMoneyAccumulated { get => _onMoneyAccumulated; set => _onMoneyAccumulated = value; }
        public Action OnUpgradeBought { get => _onUpgradeBought; set => _onUpgradeBought = value; }
        public Action OnOrderMaterialUnavaliable { get => _onOrderMaterialUnavaliable; set => _onOrderMaterialUnavaliable = value; }
        public Action OnMaterialScreenOpened { get => _onMaterialScreenOpened; set => _onMaterialScreenOpened = value; }
        public Action OnMaterialChanged { get => _onMaterialChanged; set => _onMaterialChanged = value; }
        public Action OnOrdersMaterialOpened { get => _onOrdersMaterialOpened; set => _onOrdersMaterialOpened = value; }
        public Action OnAdditionalStagesBought { get => _onAdditionalStagesBought; set => _onAdditionalStagesBought = value; }
        public Action OnSmeltingStarted { get => _onSmeltingStarted; set => _onSmeltingStarted = value; }
        public Action OnSmeltingNearTrigger { get => _onSmeltingNearTrigger; set => _onSmeltingNearTrigger = value; }
        public Action OnSmeltingNearTriggerHit { get => _onSmeltingNearTriggerHit; set => _onSmeltingNearTriggerHit = value; }
        public Action OnSmeltingGoodHit { get => _onSmeltingGoodHit; set => _onSmeltingGoodHit = value; }
        public Action OnForgingAfterSmelting { get => _onForgingAfterSmelting; set => _onForgingAfterSmelting = value; }
        public Action OnTradeStarted { get => _onTradeStarted; set => _onTradeStarted = value; }
        public Action OnTradeScreenOpened { get => _onTradeScreenOpened; set => _onTradeScreenOpened = value; }
        public Action OnTradeGreenZone { get => _onTradeGreenZone; set => _onTradeGreenZone = value; }
        public Action OnTradeFinished { get => _onTradeFinished; set => _onTradeFinished = value; }
        public Action OnHardeningStarted { get => _onHardeningStarted; set => _onHardeningStarted = value; }
        public Action OnHardeningGoodHit { get => _onHardeningGoodHit; set => _onHardeningGoodHit = value; }
        public Action OnSharpeningStarted { get => _onSharpeningStarted; set => _onSharpeningStarted = value; }
        public Action OnSharpeningGoodHit { get => _onSharpeningGoodHit; set => _onSharpeningGoodHit = value; }
        public Action OnSharpeningBadHit { get => _onSharpeningBadHit; set => _onSharpeningBadHit = value; }
        
    }
}