using Zenject;
using GameCoreModule;
using UnityEngine;

public class CoreGameInstaller : MonoInstaller
{
    [SerializeField] private PrefabsContainer _prefabsContainer;
    [SerializeField] private SceneViewsLinks _sceneViewsLinks;
    [SerializeField] private CanvasList _canvasList;
    [SerializeField] private GameConfig _gameConfig;

    public override void InstallBindings()
    {
        InstallEventBuses();
        InstallContainers();
        InstallActions();
        InstallOperators();
    }

    private void InstallEventBuses()
    {
        Container.Bind<GameEventBus>().AsSingle();
        Container.Bind<ForgingEventBus>().AsSingle();
        Container.Bind<ResultsEventBus>().AsSingle();
        Container.Bind<ProgressionEvents>().AsSingle();
        Container.Bind<OrdersEventBus>().AsSingle();
        Container.Bind<StateEventsBus>().AsSingle();
    }

    private void InstallContainers()
    {
        Container.Bind<SceneViewsContainer>().AsSingle();
        Container.Bind<PrefabsContainer>().FromInstance(_prefabsContainer).AsSingle();
        Container.Bind<SceneViewsLinks>().FromInstance(_sceneViewsLinks).AsSingle();
        Container.Bind<CanvasList>().FromInstance(_canvasList).AsSingle();
        Container.Bind<GameConfig>().FromInstance(_gameConfig).AsSingle();
    }

    private void InstallActions()
    {
        Container.Bind<GameStateActions>().AsSingle();
        Container.Bind<GameControlActions>().AsSingle();
    }

    private void InstallOperators()
    {
        Container.Bind<PoolsContainer>().AsSingle();
        Container.Bind<PoolsOperator>().AsSingle();
        Container.Bind<SpawnOperator>().AsSingle();
    }
}