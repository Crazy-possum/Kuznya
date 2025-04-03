namespace GameCoreModule
{
    public class SavableString : ISavableData<string>
    {
        private string _value;
        private bool _isLoaded;
        
        public string Value => _value;
        public bool IsLoaded => _isLoaded;

        
        public void SetValue(string value)
        {
            _value = value;
        }
    }
}