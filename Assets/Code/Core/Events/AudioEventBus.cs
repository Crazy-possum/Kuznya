using System;

namespace GameCoreModule
{
    public class AudioEventBus
    {
        private Action<AudioResourceID> _onPlaySound;
        private Action<AudioResourceID> _onPlaySound2;
        private Action<AudioResourceID> _onPlaySoundLoop;
        private Action<AudioResourceID> _onPlayAmbientSound;
        private Action<AudioResourceID> _onPlayMusic;
        private Action _onStopSound;
        private Action _onStopSound2;
        private Action _onStopAmbientSound;
        private Action<AudioResourceID, float> _onPlayMusicOneShot;
        private Action<AudioResourceID> _onPlayMusicWithPauseCurrent;
        private Action<AudioCallback> _onGetAudioSettings;
        private Action<float> _onSetMusicVolume;
        private Action<float> _onSetSoundVolume;
        
        public Action<AudioResourceID> OnPlaySound
        { get => _onPlaySound; set => _onPlaySound = value; }
        public Action<AudioResourceID> OnPlaySound2
        { get => _onPlaySound2; set => _onPlaySound2 = value; }
        public Action<AudioResourceID> OnPlaySoundLoop
        { get => _onPlaySoundLoop; set => _onPlaySoundLoop = value; }
        public Action<AudioResourceID> OnPlayAmbientSound
        { get => _onPlayAmbientSound; set => _onPlayAmbientSound = value; }
        public Action<AudioResourceID> OnPlayMusic
        { get => _onPlayMusic; set => _onPlayMusic = value; }
        public Action OnStopSound
        { get => _onStopSound; set => _onStopSound = value; }
        public Action OnStopSound2
        { get => _onStopSound2; set => _onStopSound2 = value; }
        public Action OnStopAmbientSound
        { get => _onStopAmbientSound; set => _onStopAmbientSound = value; }
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