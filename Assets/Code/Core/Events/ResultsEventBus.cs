using System;

namespace GameCoreModule
{
    public class ResultsEventBus
    {
        private Action<int> _onResultsFinished;

        public Action<int> OnResultsFinished { get => _onResultsFinished; set => _onResultsFinished = value; }
    }
}
