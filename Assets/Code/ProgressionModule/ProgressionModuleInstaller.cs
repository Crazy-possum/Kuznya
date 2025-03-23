using UnityEngine;
using Zenject;

namespace Progression
{
    public class ProgressionModuleInstaller : MonoInstaller
    {
        [SerializeField] private ProgressionContainer _progressionContainer;

        public override void InstallBindings()
        {
            Container.Bind<ProgressionContainer>().
                FromInstance(_progressionContainer).AsSingle();
            
            Container.Bind<ProgressionData>().AsSingle();
            Container.Bind<ProgressionOperator>().AsSingle();
        }
    }
}
