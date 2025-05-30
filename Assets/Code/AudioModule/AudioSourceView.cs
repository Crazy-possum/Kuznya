using UnityEngine;

namespace Audio
{
    public class AudioSourceView : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundSource;
        [SerializeField] private AudioSource _sound2Source;
        [SerializeField] private AudioSource _ambientSource;
        
        public AudioSource MusicSource => _musicSource;
        public AudioSource SoundSource => _soundSource;
        public AudioSource Sound2Source => _sound2Source;
        public AudioSource AmbientSource => _ambientSource;
    }
}

