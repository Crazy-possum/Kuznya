using System;

namespace GameCoreModule
{
    public class SaveLoadEventBus
    {
        private Action _onSavableDataChanged;
        private Action _onSaveData;
        private Action _onLoadData;

        private Action<SaveDataKey, int> _onSaveInt;
        private Action<SaveDataKey, float> _onSaveFloat;
        private Action<SaveDataKey, string> _onSaveString;

        public Action<SaveDataKey, SavableInt> _onGetInt;
        private Action<SaveDataKey, SavableFloat> _onGetFloat;
        private Action<SaveDataKey, SavableString> _onGetString;
        

        public Action OnSavableDataChanged
        { get => _onSavableDataChanged; set => _onSavableDataChanged = value; }
        public Action OnSaveData
        { get => _onSaveData; set => _onSaveData = value; }
        public Action OnLoadData
        { get => _onLoadData; set => _onLoadData = value; }
        
        public Action<SaveDataKey, int> OnSaveInt
        { get => _onSaveInt; set => _onSaveInt = value; }
        public Action<SaveDataKey, float> OnSaveFloat
        { get => _onSaveFloat; set => _onSaveFloat = value; }
        public Action<SaveDataKey, string> OnSaveString
        { get => _onSaveString; set => _onSaveString = value; }
        
        public Action<SaveDataKey, SavableInt> OnGetInt
        { get => _onGetInt; set => _onGetInt = value; }
        public Action<SaveDataKey, SavableFloat> OnGetFloat
        { get => _onGetFloat; set => _onGetFloat = value; }
        public Action<SaveDataKey, SavableString> OnGetString
        { get => _onGetString; set => _onGetString = value; }
        
    }
}