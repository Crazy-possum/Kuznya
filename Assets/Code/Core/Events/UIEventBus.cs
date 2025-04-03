using System;
using UnityEngine;

namespace GameCoreModule
{
    public class UIEventBus
    {
        private Action<PointerCheckEventCallback> _onPointerCheck;
        public Action _onFreezeUI;
        public Action _onUnfreezeUI;
        
        public Action<PointerCheckEventCallback> OnPointerCheck
        { get => _onPointerCheck; set => _onPointerCheck = value; }
        public Action OnFreezeUI
        { get => _onFreezeUI; set => _onFreezeUI = value; }
        public Action OnUnfreezeUI
        { get => _onUnfreezeUI; set => _onUnfreezeUI = value; }
    }
}