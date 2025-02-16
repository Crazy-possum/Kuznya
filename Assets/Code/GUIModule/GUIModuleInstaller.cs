using UnityEngine;
using Zenject;

namespace MainGUI
{
    public class GUIModuleInstaller : MonoInstaller
    {
        [SerializeField] private GUIView _guiView;

        public override void InstallBindings()
        {
            Container.Bind<GUIView>().
                FromInstance(_guiView).AsSingle();
        }
    }
}

