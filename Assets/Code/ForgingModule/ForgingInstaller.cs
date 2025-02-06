using UnityEngine;
using Zenject;

public class ForgingInstaller : MonoInstaller
{
    [SerializeField] private ForgingUIView _uiView;
    

    public override void InstallBindings()
    {
        Container.Bind<ForgingUIView>().FromInstance(_uiView).AsSingle();

        Container.Bind<ForgingActions>().AsSingle();
        

    }
}