using GameCoreModule;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;
    private AudioEventBus _audioEventBus;
    private SaveLoadEventBus _saveLoadEventBus;

    [Inject]
    private void Construct(AudioEventBus audioEventBus, SaveLoadEventBus saveLoadEventBus)
    {
        _audioEventBus = audioEventBus;
        _saveLoadEventBus = saveLoadEventBus;
    }
    
    private void Start()
    {
        GetCurentVolumeSettings();

        _soundSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
        _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
    }
    

    private void GetCurentVolumeSettings()
    {
        AudioCallback audioCallback = new AudioCallback();
        _audioEventBus.OnGetAudioSettings?.Invoke(audioCallback);
        _soundSlider.value = audioCallback.SoundVolume;
        _musicSlider.value = audioCallback.MusicVolume;
    }
    
    private void OnSoundVolumeChanged(float soundVolume)
    {
        _audioEventBus.OnSetSoundVolume(soundVolume);
    }
    
    private void OnMusicVolumeChanged(float musicVolume)
    {
        _audioEventBus.OnSetMusicVolume(musicVolume);
    }
	
	public void ClearProgress()
	{
        _saveLoadEventBus.OnClearData?.Invoke();
        SceneManager.LoadScene(0);
    }
}
