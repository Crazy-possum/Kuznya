using Zenject;
using GameCoreModule;
using UnityEngine;

public class CoreGameInstaller : MonoInstaller
{
    [SerializeField] private PrefabsContainer _prefabsContainer;
    [SerializeField] private SceneViewsLinks _sceneViewsLinks;

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
    }

    private void InstallContainers()
    {
        Container.Bind<SceneViewsContainer>().AsSingle();
        Container.Bind<PrefabsContainer>().FromInstance(_prefabsContainer).AsSingle();
        Container.Bind<SceneViewsLinks>().FromInstance(_sceneViewsLinks).AsSingle();
    }

    private void InstallActions()
    {
        Container.Bind<GameStateActions>().AsSingle();
    }

    private void InstallOperators()
    {
        Container.Bind<PoolsContainer>().AsSingle();
        Container.Bind<PoolsOperator>().AsSingle();
        Container.Bind<SpawnOperator>().AsSingle();
    }
}