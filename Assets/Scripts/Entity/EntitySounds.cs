using UnityEngine;

namespace Entity
{
    [RequireComponent(typeof(AudioSource))]
    public class EntitySounds : MonoBehaviour
    {
        private AudioSource _source;
        
        private void Awake() => _source = GetComponent<AudioSource>();

        /// <summary>
        /// Plays an audio clip
        /// </summary>
        /// <param name="clip">The audio clip to play</param>
        private void PlaySound(AudioClip clip)
        {
            _source.clip = clip;
            _source.Play();
        }
    }
}