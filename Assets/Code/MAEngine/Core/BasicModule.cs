using UnityEngine;
using Zenject;

namespace MAEngine
{
    public class BasicModule : MonoBehaviour, IModule
    {
        internal Actions _actions;
        internal DiContainer _di;

        [Inject]
        public void Construct(DiContainer di)
        {
            _di = di;
        }

        public IActions Actions => _actions;

        public virtual void Initialise()
        {
            _actions = new Actions();
        }
    }
}

