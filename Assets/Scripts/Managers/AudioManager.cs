using System.Collections;
using System.Collections.Generic;
using GameAudio;
using UnityEngine;

namespace Managers
{
    /**
     * Play sounds with 'AudioManager.Instance.Play("name");'
     */
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField][Tooltip("Container for all sounds")]
        private List<Sound> sounds;

        [SerializeField]
        private GameObject audioSources;

        protected override void Awake()
        {
            base.Awake();
                
            // Setting up audio sources
            foreach (Sound s in sounds)
            {
                s.source = audioSources.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.loop = s.loop;
            }
        }
        
        /// <summary>
        /// Searches for a sound and plays it
        /// </summary>
        public void Play(string soundName)
        {
            FindSound(soundName).source.Play();
        }
        
        /// <summary>
        /// Searches for a sound and fades it in
        /// </summary>
        public void FadeIn(string soundName, float duration)
        {
            StartCoroutine(FadeInIe(FindSound(soundName), duration));
        }
        
        /// <summary>
        /// Searches for a sound and fades it in
        /// </summary>
        public void FadeOut(string soundName, float duration)
        {
            StartCoroutine(FadeOutIe(FindSound(soundName), duration));
        }
        
        /// <summary>
        /// Returns a sound by a given name
        /// </summary>
        private Sound FindSound(string soundName)
        {
            Sound soundToFind= sounds.Find(sound => sound.name == soundName);
            if (soundToFind == null)
            {
                Debug.Log("Unable to find sound: " + soundName);
                return null;
            }
            return soundToFind;
        }
        
        /// <summary>
        /// Fades in a sound if called via Coroutine 
        /// </summary>
        private IEnumerator FadeInIe(Sound soundToFade, float duration)
        {
            float finalVolume = soundToFade.volume;
            float currentTime = 0f;
            
            soundToFade.source.volume = 0f;
            soundToFade.source.Play();

            while (currentTime < duration)
            {
                float volume = Mathf.Lerp(0f, finalVolume, currentTime / duration);
                soundToFade.source.volume = volume;
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
        
        /// <summary>
        /// Fades out a sound if called via Coroutine 
        /// </summary>
        private IEnumerator FadeOutIe(Sound soundToFade, float duration)
        {
            float startVolume = soundToFade.volume;
            float currentTime = 0f;
            
            while (currentTime < duration)
            {
                float volume = Mathf.Lerp(startVolume, 0f, currentTime / duration);
                soundToFade.source.volume = volume;
                currentTime += Time.deltaTime;
                yield return null;
            }
            soundToFade.source.Stop();
        }
    }
}