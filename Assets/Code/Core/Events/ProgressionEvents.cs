using Progression;
using System;

namespace GameCoreModule
{
    public class ProgressionEvents
    {
        private Action<ProgressionData> onProgressionDataLoaded;
        private Action<ProgressionData> onProgressionDataChanged;

        public Action<ProgressionData> OnProgressionDataLoaded { get => onProgressionDataLoaded; set => onProgressionDataLoaded = value; }
        public Action<ProgressionData> OnProgressionDataChanged { get => onProgressionDataChanged; set => onProgressionDataChanged = value; }
    }
}
