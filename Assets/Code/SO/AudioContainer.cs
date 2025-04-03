using MAEngine.Extention;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "AudioContainer", menuName = "SO/Containers/AudioContainer")]
    public class AudioContainer : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<AudioResourceID, AudioClip> _audioDict;
        
        public SerializableDictionary<AudioResourceID, AudioClip> AudioDict { get => _audioDict; }

        public AudioClip GetAudioClip(AudioResourceID id)
        {
            AudioClip clip = _audioDict[id];
            if (clip == null)
            {
                Debug.LogWarning($"No audio with ID : {id}");
            }
            return clip;
        }
    }
}