using Progression;
using System;

namespace GameCoreModule
{
    public class ProgressionEvents
    {
        private Action<ProgressionData> onProgressionDataLoaded;
        private Action<ProgressionData> onProgressionDataChanged;
        private Action onProgressionDataCleared;

        public Action<ProgressionData> OnProgressionDataLoaded { get => onProgressionDataLoaded; set => onProgressionDataLoaded = value; }
        public Action<ProgressionData> OnProgressionDataChanged { get => onProgressionDataChanged; set => onProgressionDataChanged = value; }
        public Action OnProgressionDataCleared { get => onProgressionDataCleared; set => onProgressionDataCleared = value; }
    }
}
