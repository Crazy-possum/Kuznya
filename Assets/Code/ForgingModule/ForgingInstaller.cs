using UnityEngine;
using Zenject;

public class ForgingInstaller : MonoInstaller
{
    [SerializeField] private ForgingUIView _forgingView;
    [SerializeField] private ResultsUIView _resultsView;
    
    [SerializeField] private OrderSpritesContainer _orderSpritesContainer;
    

    public override void InstallBindings()
    {
        Container.Bind<ForgingUIView>().FromInstance(_forgingView).AsSingle();
        Container.Bind<ResultsUIView>().FromInstance(_resultsView).AsSingle();
        
        Container.Bind<OrderSpritesContainer>().FromInstance(_orderSpritesContainer).AsSingle();

        Container.Bind<ForgingActions>().AsSingle();
        Container.Bind<ResultsActions>().AsSingle();
        

    }
}