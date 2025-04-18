using System;

namespace GameCoreModule
{
    public class StateEventsBus
    {
        private Action _onDialogueStateActivate;
        private Action _onOrdersStateActivate;
        private Action _onShopStateActivate;
        private Action _onMaterialsStateActivate;
        private Action<float> _onForgingStateActivate;
        private Action _onResultsStateActivate;
        private Action _onTradeStateActivate;
        private Action _onSmeltingStateActivate;
        private Action _onHardeningStateActivate;

        public Action OnDialogueStateActivate { get => _onDialogueStateActivate; set => _onDialogueStateActivate = value; }
        public Action OnOrdersStateActivate { get => _onOrdersStateActivate; set => _onOrdersStateActivate = value; }
        public Action OnShopStateActivate { get => _onShopStateActivate; set => _onShopStateActivate = value; }
        public Action OnMaterialsStateActivate { get => _onMaterialsStateActivate; set => _onMaterialsStateActivate = value; }
        public Action<float> OnForgingStateActivate { get => _onForgingStateActivate; set => _onForgingStateActivate = value; }
        public Action OnResultsStateActivate { get => _onResultsStateActivate; set => _onResultsStateActivate = value; }
        public Action OnTradeStateActivate { get => _onTradeStateActivate; set => _onTradeStateActivate = value; }
        public Action OnSmeltingStateActivate { get => _onSmeltingStateActivate; set => _onSmeltingStateActivate = value; }
        public Action OnHardeningStateActivate { get => _onHardeningStateActivate; set => _onHardeningStateActivate = value; }
    }
}
