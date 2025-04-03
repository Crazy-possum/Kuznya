namespace GameCoreModule
{
    public interface ISavableData<T>
    {
        public T Value { get; }
        public bool IsLoaded { get; }
        
        
        public void SetValue(T value);
    }
}