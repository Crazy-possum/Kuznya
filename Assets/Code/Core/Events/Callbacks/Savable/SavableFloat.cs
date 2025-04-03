namespace GameCoreModule
{
    public class SavableFloat : ISavableData<float>
    {
        private float _value;
        private bool _isLoaded;
        
        public float Value => _value;
        public bool IsLoaded => _isLoaded;
        
        
        public void SetValue(float value)
        {
            _value = value;
            _isLoaded = true;
        }
    }
}