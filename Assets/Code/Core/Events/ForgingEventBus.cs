using System;

namespace GameCoreModule
{
    public class ForgingEventBus
    {
        private Action<int> _onForgingFinished;
        private Action<int> _onSharpeningFinished;

        public Action<int> OnForgingFinished { get => _onForgingFinished; set => _onForgingFinished = value; }
        public Action<int> OnSharpeningFinished { get => _onSharpeningFinished; set => _onSharpeningFinished = value; }
    }
}
