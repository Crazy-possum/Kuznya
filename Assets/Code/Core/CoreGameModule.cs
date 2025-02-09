using MAEngine;
using System;

namespace GameCoreModule
{
    public class CoreGameModule : BasicModule
    {
        public override void Initialise()
        {
            InitializeFields();
            InitializeGameState();
            InitializePoolsOperator();
            InitializeSpawnOperator();
            InitializeGameControl();
        }

        private void InitializeFields()
        {
            _actions = new Actions();
        }

        private void InitializeGameState()
        {
            GameStateActions stateActions =
                _di.Resolve<GameStateActions>();
            _actions.Add(stateActions);
        }

        private void InitializePoolsOperator()
        {
            PoolsOperator poolsOperator =
                _di.Resolve<PoolsOperator>();
            _actions.Add(poolsOperator);
        }

        private void InitializeSpawnOperator()
        {
            SpawnOperator spawnOperator =
                _di.Resolve<SpawnOperator>();
            _actions.Add(spawnOperator);
        }

        private void InitializeGameControl()
        {
            GameControlActions gameControl = 
                _di.Resolve<GameControlActions>();
            _actions.Add(gameControl);
        }

    }
}

