using System;

namespace Progression
{
    [Serializable]
    public class Unlocks
    {
        private bool _isSmeltingUnlocked;
        private bool _isTradeUnlocked;
        private bool _isSharpeningUnlocked;
        private bool _isHardeningUnlocked;
        private bool _isFinalizationUnlocked;
        
        public bool IsSmeltingUnlocked { get => _isSmeltingUnlocked; set => _isSmeltingUnlocked = value; }
        public bool IsTradeUnlocked { get => _isTradeUnlocked; set => _isTradeUnlocked = value; }
        public bool IsSharpeningUnlocked { get => _isSharpeningUnlocked; set => _isSharpeningUnlocked = value; }
        public bool IsHardeningUnlocked { get => _isHardeningUnlocked; set => _isHardeningUnlocked = value; }
        public bool IsFinalizationUnlocked { get => _isFinalizationUnlocked; set => _isFinalizationUnlocked = value; }
        
    }
}