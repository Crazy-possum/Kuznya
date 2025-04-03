namespace GameCoreModule
{
    public class SavableInt : ISavableData<int>
    {
        private int _value;
        private bool _isLoaded;
        
        public int Value => _value;
        public bool IsLoaded => _isLoaded;

        
        public void SetValue(int value)
        {
            _value = value;
        }
    }
}