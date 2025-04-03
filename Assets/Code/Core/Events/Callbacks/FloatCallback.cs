namespace GameCoreModule
{
    public class FloatCallback
    {
        private float _floatValue;
        
        public float FloatValue => _floatValue;
        
        public void SendDataToCallback(float value)
        {
            _floatValue = value;
        }
    }
}