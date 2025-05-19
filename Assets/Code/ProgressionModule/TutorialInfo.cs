using System;

namespace Progression
{
    [Serializable]
    public class TutorialInfo
    {
        private bool _isGameStarted;
        private bool _isTutorialStarted;
        private bool _isTutorialSkiped;
        private bool _isClientActivated;
        private bool _isDialogueStarted;
        private bool _isOrderApplied;
        private bool _isOrdersScreenOpened;
        private bool _isForgingStarted;
        private bool _isForgingHit;
        private bool _isForgingStageChanged;
        private bool _isResultScreenOpened;
        private bool _isOrderFinished;
        private bool _isOrderSubmitStarted;
        private bool _isMoneyAdded;
        private bool _isShopScreenOpened;
        private bool _isMoneyAccumulated;
        private bool _isUpgradeBought;
        private bool _isOrderMaterialUnavailable;
        private bool _isMaterialScreenOpened;
        private bool _isMaterialChanged;
        private bool _isOrdersMaterialOpened;
        private bool _isAdditionalStagesBought;
        private bool _isSmeltingStarted;
        private bool _isSmeltingNearTrigger;
        private bool _isSmeltingGoodHit;
        private bool _isForgingAfterSmelting;
        private bool _isTradeStarted;
        private bool _isTradeScreenOpened;
        private bool _isTradeGreenZone;
        private bool _isTradeFinished;
        private bool _isHardeningStarted;
        private bool _isHardeningGoodHit;
        private bool _isSharpeningStarted;
        private bool _isSharpeningGoodHit;
        private bool _isSharpeningBadHit;
        
        public bool IsGameStarted { get => _isGameStarted; set => _isGameStarted = value; }
        public bool IsTutorialStarted { get => _isTutorialStarted; set => _isTutorialStarted = value; }
        public bool IsTutorialSkiped { get => _isTutorialSkiped; set => _isTutorialSkiped = value; }
        public bool IsClientActivated { get => _isClientActivated; set => _isClientActivated = value; }
        public bool IsDialogueStarted { get => _isDialogueStarted; set => _isDialogueStarted = value; }
        public bool IsOrderApplied { get => _isOrderApplied; set => _isOrderApplied = value; }
        public bool IsOrdersScreenOpened { get => _isOrdersScreenOpened; set => _isOrdersScreenOpened = value; }
        public bool IsForgingStarted { get => _isForgingStarted; set => _isForgingStarted = value; }
        public bool IsForgingHit { get => _isForgingHit; set => _isForgingHit = value; }
        public bool IsForgingStageChanged { get => _isForgingStageChanged; set => _isForgingStageChanged = value; }
        public bool IsResultScreenOpened { get => _isResultScreenOpened; set => _isResultScreenOpened = value; }
        public bool IsOrderFinished { get => _isOrderFinished; set => _isOrderFinished = value; }
        public bool IsOrderSubmitStarted { get => _isOrderSubmitStarted; set => _isOrderSubmitStarted = value; }
        public bool IsMoneyAdded { get => _isMoneyAdded; set => _isMoneyAdded = value; }
        public bool IsShopScreenOpened { get => _isShopScreenOpened; set => _isShopScreenOpened = value; }
        public bool IsMoneyAccumulated { get => _isMoneyAccumulated; set => _isMoneyAccumulated = value; }
        public bool IsUpgradeBought { get => _isUpgradeBought; set => _isUpgradeBought = value; }
        public bool IsOrderMaterialUnavailable { get => _isOrderMaterialUnavailable; set => _isOrderMaterialUnavailable = value; }
        public bool IsMaterialScreenOpened { get => _isMaterialScreenOpened; set => _isMaterialScreenOpened = value; }
        public bool IsMaterialChanged { get => _isMaterialChanged; set => _isMaterialChanged = value; }
        public bool IsOrdersMaterialOpened { get => _isOrdersMaterialOpened; set => _isOrdersMaterialOpened = value; }
        public bool IsAdditionalStagesBought { get => _isAdditionalStagesBought; set => _isAdditionalStagesBought = value; }
        public bool IsSmeltingStarted { get => _isSmeltingStarted; set => _isSmeltingStarted = value; }
        public bool IsSmeltingNearTrigger { get => _isSmeltingNearTrigger; set => _isSmeltingNearTrigger = value; }
        public bool IsSmeltingGoodHit { get => _isSmeltingGoodHit; set => _isSmeltingGoodHit = value; }
        public bool IsForgingAfterSmelting { get => _isForgingAfterSmelting; set => _isForgingAfterSmelting = value; }
        public bool IsTradeStarted { get => _isTradeStarted; set => _isTradeStarted = value; }
        public bool IsTradeScreenOpened { get => _isTradeScreenOpened; set => _isTradeScreenOpened = value; }
        public bool IsTradeGreenZone { get => _isTradeGreenZone; set => _isTradeGreenZone = value; }
        public bool IsTradeFinished { get => _isTradeFinished; set => _isTradeFinished = value; }
        public bool IsHardeningStarted { get => _isHardeningStarted; set => _isHardeningStarted = value; }
        public bool IsHardeningGoodHit { get => _isHardeningGoodHit; set => _isHardeningGoodHit = value; }
        public bool IsSharpeningStarted { get => _isSharpeningStarted; set => _isSharpeningStarted = value; }
        public bool IsSharpeningGoodHit { get => _isSharpeningGoodHit; set => _isSharpeningGoodHit = value; }
        public bool IsSharpeningBadHit { get => _isSharpeningBadHit; set => _isSharpeningBadHit = value; }
    }
}