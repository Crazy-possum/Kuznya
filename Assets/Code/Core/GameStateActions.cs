using MAEngine;
using System;
using UnityEngine;
using Zenject;

namespace GameCoreModule
{
    public class GameStateActions : IAction, IInitialisation, ICleanUp
    {
        private GameEventBus _gameEventBus;
        private GameState _currentGameState;

        [Inject]
        public void Construct(GameEventBus gameEventBus)
        {
            _gameEventBus = gameEventBus;
        }

        public void Initialisation()
        {
            _gameEventBus.OnContinueGame += SetPlayingState;
            SetPlayingState();
        }

        public void Cleanup()
        {
            _gameEventBus.OnContinueGame -= SetPlayingState;
        }

        private void SetPlayingState()
        {
            if (_currentGameState != GameState.PlayState)
            {
                Time.timeScale = 1f;
                _currentGameState = GameState.PlayState;
                _gameEventBus.OnStateChanged?.Invoke(_currentGameState);
            }
        }
        private void SetPauseState()
        {
            Debug.Log("Paused");
            if (_currentGameState != GameState.PauseState)
            {
                Time.timeScale = 0f;
                _currentGameState = GameState.PauseState;
                _gameEventBus.OnStateChanged?.Invoke(_currentGameState);
            }
        }
    }
}
