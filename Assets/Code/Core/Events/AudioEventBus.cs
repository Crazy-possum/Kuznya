using System;

namespace GameCoreModule
{
    public class AudioEventBus
    {
        private Action<AudioResourceID> _onPlaySound;
        private Action<AudioResourceID> _onPlayMusic;
        private Action<AudioResourceID, float> _onPlayMusicOneShot;
        private Action<AudioResourceID> _onPlayMusicWithPauseCurrent;
        private Action<AudioCallback> _onGetAudioSettings;
        private Action<float> _onSetMusicVolume;
        private Action<float> _onSetSoundVolume;
        
        public Action<AudioResourceID> OnPlaySound
        { get => _onPlaySound; set => _onPlaySound = value; }
        public Action<AudioResourceID> OnPlayMusic
        { get => _onPlayMusic; set => _onPlayMusic = value; }
        public Action<AudioResourceID, float> OnPlayMusicOneShot
        { get => _onPlayMusicOneShot; set => _onPlayMusicOneShot = value; }
        public Action<AudioResourceID> OnPlayMusicWithPauseCurrent
        { get => _onPlayMusicWithPauseCurrent; set => _onPlayMusicWithPauseCurrent = value; }
        public Action<AudioCallback> OnGetAudioSettings
        { get => _onGetAudioSettings; set => _onGetAudioSettings = value; }
        public Action<float> OnSetMusicVolume
        { get => _onSetMusicVolume; set => _onSetMusicVolume = value; }
        public Action<float> OnSetSoundVolume
        { get => _onSetSoundVolume; set => _onSetSoundVolume = value; }
    }
}