using UnityEngine;

namespace Audio
{
    public class AudioSourceView : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundSource;
        
        public AudioSource MusicSource => _musicSource;
        public AudioSource SoundSource => _soundSource;
    }
}

