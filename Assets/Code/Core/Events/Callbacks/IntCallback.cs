namespace GameCoreModule
{
    public class IntCallback
    {
        private int _intValue;
        
        public int INTValue => _intValue;
        
        public void SendDataToCallback(int value)
        {
            _intValue = value;
        }
    }
}