using System;

namespace GameCoreModule
{
    public class ForgingEventBus
    {
        private Action<int> _onForgingFinished;
        private Action<int> _onHardeningFinished;

        public Action<int> OnForgingFinished { get => _onForgingFinished; set => _onForgingFinished = value; }
        public Action<int> OnHardeningFinished { get => _onHardeningFinished; set => _onHardeningFinished = value; }
    }
}
