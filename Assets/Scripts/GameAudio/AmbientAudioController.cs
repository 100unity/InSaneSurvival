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

        private void OnEnable()
        {
            Clock.SunRise += DayAmbience;
            Clock.SunSet += NightAmbience;
            Clock.MoonRise += MoonRiseAmbience;
            Clock.MoonSet += MoonShineAmbienceStop;
        }

        private void OnDisable()
        {
            Clock.SunRise -= DayAmbience;
            Clock.SunSet -= NightAmbience;
            Clock.MoonRise -= MoonRiseAmbience;
            Clock.MoonSet -= MoonShineAmbienceStop;
        }

        private void Start()
        {
            AudioManager.Instance.FadeIn(clock.IsNight ? "AmbCrickets" : "AmbBirds", 5);
        }

        private void DayAmbience()
        {
            AudioManager.Instance.FadeIn("AmbBirds",120);
        }

        private void NightAmbience()
        {
            AudioManager.Instance.FadeOut("AmbBirds",10);
            AudioManager.Instance.FadeIn("AmbCrickets",45);
        }

        private void MoonRiseAmbience()
        {
            AudioManager.Instance.Play("AmbOwl");
            
        }

        private void MoonShineAmbienceStop()
        {
            AudioManager.Instance.FadeOut("AmbOwl",5);
            AudioManager.Instance.FadeOut("AmbCrickets",30);
        }
    }
}
