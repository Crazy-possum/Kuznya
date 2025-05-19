using MAEngine;

namespace GameCoreModule
{
    public class PoolCallback
    {
        private IPool _pool;
        
        public IPool Pool => _pool;

        public void SetPool(IPool pool)
        {
            _pool = pool;
        }
    }
}