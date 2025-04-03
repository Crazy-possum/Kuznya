using GameCoreModule;
using MAEngine;
using MAEngine.Extention;
using SO;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class AudioOperator : IAction, IInitialisation, ICleanUp, IFixedExecute
    {
        private AudioSourceView _sourceView;
        private AudioEventBus _audioEventBus;
        private AudioContainer _audioContainer;
        private SaveLoadEventBus _saveLoadEventBus;

        private float _currentPauseTime;
        private Timer _musicPauseTimer;
        private AudioClip _pausedMusicClip;

        [Inject]
        public void Construct(AudioSourceView sourceView, AudioEventBus audioEventBus,
            AudioContainer audioContainer, SaveLoadEventBus saveLoadEventBus)
        {
            _sourceView = sourceView;
            _audioEventBus = audioEventBus;
            _audioContainer = audioContainer;
            _saveLoadEventBus = saveLoadEventBus;
        }
        
        public void Initialisation()
        {
            _audioEventBus.OnPlayMusic += PlayMusic;
            _audioEventBus.OnPlaySound += PlaySound;
            _audioEventBus.OnPlayMusicOneShot += PlayMusicOneShot;
            _audioEventBus.OnPlayMusicWithPauseCurrent += PlayMusicWithPauseCurrent;
            _audioEventBus.OnGetAudioSettings += GetAudioSettings;
            _audioEventBus.OnSetMusicVolume += SetMusicVolume;
            _audioEventBus.OnSetSoundVolume += SetSoundVolume;
            LoadAudioSettings();
            //_audioEventBus.OnPlayMusic?.Invoke(AudioResourceID.Music_Ambient_1);
        }

        public void Cleanup()
        {
            _audioEventBus.OnPlayMusic -= PlayMusic;
            _audioEventBus.OnPlaySound -= PlaySound;
            _audioEventBus.OnPlayMusicOneShot -= PlayMusicOneShot;
            _audioEventBus.OnPlayMusicWithPauseCurrent -= PlayMusicWithPauseCurrent;
            _audioEventBus.OnGetAudioSettings -= GetAudioSettings;
            _audioEventBus.OnSetMusicVolume -= SetMusicVolume;
            _audioEventBus.OnSetSoundVolume -= SetSoundVolume;
        }
        
        public void FixedExecute(float deltaTime)
        {
            if (_musicPauseTimer != null)
            {
                if (_musicPauseTimer.Wait())
                {
                    ResumeMusic();
                    _musicPauseTimer = null;
                }
            }
        }
        
        private void LoadAudioSettings()
        {
            SavableFloat musicVolumeFloat = new SavableFloat();
            SavableFloat soundVolumeFloat = new SavableFloat();
            _saveLoadEventBus.OnGetFloat?.Invoke(SaveDataKey.MusicVolume, musicVolumeFloat);
            _saveLoadEventBus.OnGetFloat?.Invoke(SaveDataKey.SoundVolume, soundVolumeFloat);
            if (musicVolumeFloat.IsLoaded)
            {
                _sourceView.MusicSource.volume = musicVolumeFloat.Value;
            }

            if (soundVolumeFloat.IsLoaded)
            {
                _sourceView.SoundSource.volume = soundVolumeFloat.Value;
            }
        }

        private void SaveAudioSettings()
        {
            _saveLoadEventBus.OnSaveFloat?.Invoke(SaveDataKey.MusicVolume, _sourceView.MusicSource.volume);
            _saveLoadEventBus.OnSaveFloat?.Invoke(SaveDataKey.SoundVolume, _sourceView.SoundSource.volume);
            _saveLoadEventBus.OnSaveData?.Invoke();
        }

        private void PlaySound(AudioResourceID id)
        {
            _sourceView.SoundSource.PlayOneShot(_audioContainer.GetAudioClip(id));
        }

        private void PlayMusic(AudioResourceID id)
        {
            _sourceView.MusicSource.Stop();
            _sourceView.MusicSource.clip = _audioContainer.GetAudioClip(id);
            _sourceView.MusicSource.Play();
        }

        private void PlayMusicOneShot(AudioResourceID id, float pauseTime)
        {
            PauseMusic(pauseTime);
            _sourceView.MusicSource.clip = _audioContainer.GetAudioClip(id);
            _sourceView.MusicSource.Play();
        }
        
        private void PlayMusicWithPauseCurrent(AudioResourceID id)
        {
            PauseMusic();
            _sourceView.MusicSource.clip = _audioContainer.GetAudioClip(id);
            _sourceView.MusicSource.Play();
        }

        private void PauseMusic()
        {
            _pausedMusicClip = _sourceView.MusicSource.clip;
            _sourceView.MusicSource.Stop();
        }
        
        private void PauseMusic(float pauseTime)
        {
            _currentPauseTime = _sourceView.MusicSource.time;
            _musicPauseTimer = new Timer(pauseTime);
            _sourceView.MusicSource.Stop();
        }
        
        private void ResumeMusic()
        {
            if (_pausedMusicClip != null)
            {
                _sourceView.MusicSource.clip = _pausedMusicClip;
                _pausedMusicClip = null;
            }
            _sourceView.MusicSource.time = _currentPauseTime;
            _sourceView.MusicSource.Play();
            _sourceView.MusicSource.time = _currentPauseTime;
        }
        
        private void GetAudioSettings(AudioCallback callback)
        {
            callback.MusicVolume = _sourceView.MusicSource.volume;
            callback.SoundVolume = _sourceView.SoundSource.volume;
        }
        
        private void SetMusicVolume(float volume)
        {
            _sourceView.MusicSource.volume = volume;
            SaveAudioSettings();
        }

        private void SetSoundVolume(float volume)
        {
            _sourceView.SoundSource.volume = volume;
            SaveAudioSettings();
        }
    }
}