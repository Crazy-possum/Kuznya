using MAEngine;
using Zenject;

namespace GameCoreModule
{
    public class GameControlActions : IAction, IInitialisation, ICleanUp
    {
        private ForgingEventBus _forgingEventBus;
        private CanvasList _canvasList;

        [Inject]
        public void Construct(ForgingEventBus forgingEventBus, CanvasList canvasList)
        {
            _forgingEventBus = forgingEventBus;
            _canvasList = canvasList;
        }

        public void Initialisation()
        {
            _forgingEventBus.OnForgingFinished += ShowResultsScreen;
        }

        public void Cleanup()
        {
            _forgingEventBus.OnForgingFinished -= ShowResultsScreen;
        }

        private void ShowResultsScreen(int score)
        {
            _canvasList.ResultsCanvas.SetActive(true);
        }
    }
}
