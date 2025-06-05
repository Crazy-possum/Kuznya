using Progression;
using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    [SerializeField] private ProgressionContainer _progressionContainer;
    
    public override void InstallBindings()
    {
        Container.Bind<ProgressionContainer>().
            FromInstance(_progressionContainer).AsSingle();
        
        Container.Bind<ProgressionData>().AsSingle();
    }
}