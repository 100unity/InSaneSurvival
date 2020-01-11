using System;
using GameTime;
using Managers;
using UnityEngine;

namespace GameAudio
{
    public class AmbientAudioController : MonoBehaviour
    {
        [SerializeField][Tooltip("Clock which is managing the game time ")]
        private Clock clock;
        
        [SerializeField][Tooltip("Name of the sound that is playing during the day time")]
        private string dayTimeAmbience;
        
        [SerializeField][Tooltip("Name of the sound that is playing during the night")]
        private string nightTimeAmbience;
        
        [SerializeField][Tooltip("Name of the sound that is playing during moon light")]
        private string moonLightAmbience;

        private void OnEnable()
        {
            Clock.SunRise += DayAmbience;
            Clock.SunSet += NightAmbience;
            Clock.MoonRise += MoonRiseAmbience;
            Clock.MoonSet += StopAllNightAmbience;
        }

        private void OnDisable()
        {
            Clock.SunRise -= DayAmbience;
            Clock.SunSet -= NightAmbience;
            Clock.MoonRise -= MoonRiseAmbience;
            Clock.MoonSet -= StopAllNightAmbience;
        }

        private void Start()
        {
            AudioManager.Instance.FadeIn(clock.IsNight ? nightTimeAmbience : dayTimeAmbience, 5);
        }

        /// <summary>
        /// Plays the audio file that is declared for playing during day time
        /// </summary>
        private void DayAmbience()
        {
            AudioManager.Instance.FadeIn(dayTimeAmbience,120);
        }

        /// <summary>
        /// Plays the audio file that is declared for playing during night time, stops day time audio
        /// </summary>
        private void NightAmbience()
        {
            AudioManager.Instance.FadeOut(dayTimeAmbience,10);
            AudioManager.Instance.FadeIn(nightTimeAmbience,45);
        }
        
        /// <summary>
        /// Plays the audio file that is declared for playing during moon shine
        /// </summary>
        private void MoonRiseAmbience()
        {
            AudioManager.Instance.Play(moonLightAmbience);
            
        }
        
        /// <summary>
        /// Stops all ambient audio that is playing at night
        /// </summary>
        private void StopAllNightAmbience()
        {
            AudioManager.Instance.FadeOut(moonLightAmbience,5);
            AudioManager.Instance.FadeOut(nightTimeAmbience,30);
        }
    }
}
