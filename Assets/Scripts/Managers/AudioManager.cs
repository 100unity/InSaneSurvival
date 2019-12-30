using System.Collections;
using GameAudio;
using UnityEngine;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        /// <summary>
        /// contains all sounds
        /// Call sounds with "AudioManager.Instance.Play("name");"
        /// </summary>
        [SerializeField][Tooltip("Container for all sounds")]
        private System.Collections.Generic.List<Sound> sounds;

        [SerializeField]
        private GameObject audioSources;

        private void Start()
        {
            // Applying properties of each sound to their audio sources
            foreach (Sound s in sounds)
            {
                s.source = audioSources.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.loop = s.loop;
            }
        }
        
        public void Play(string soundName)
        {
            Sound soundToPlay = sounds.Find(sound => sound.name == soundName);
            if (soundToPlay == null)
            {
                Debug.Log("Unable to find sound: " + soundName);
                return;
            }
            soundToPlay.source.Play();
        }

        public void FadeIn(string soundName, float duration)
        {
            StartCoroutine(FadeInIe(soundName, duration));
        }
        
        public void FadeOut(string soundName, float duration)
        {
            StartCoroutine(FadeOutIe(soundName, duration));
        }
        
        private IEnumerator FadeInIe(string soundName, float duration)
        {
            Sound soundToFade = sounds.Find(sound => sound.name == soundName);
            if (soundToFade == null)
            {
                Debug.Log("Unable to find sound: " + soundName);
                yield break;
            }
            
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
        
        public IEnumerator FadeOutIe(string soundName, float duration)
        {
            Sound soundToFade = sounds.Find(sound => sound.name == soundName);
            if (soundToFade == null)
            {
                Debug.Log("Unable to find sound: " + soundName);
                yield break;
            }
            
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