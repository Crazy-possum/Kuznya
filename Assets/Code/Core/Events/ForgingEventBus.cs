using System;

namespace GameCoreModule
{
    public class ForgingEventBus
    {
        private Action<int> _onForgingFinished;

        public Action<int> OnForgingFinished { get => _onForgingFinished; set => _onForgingFinished = value; }
    }
}
