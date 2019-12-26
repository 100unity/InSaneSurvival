using System;
using Boo.Lang;
using GameAudio;
using UnityEngine.Audio;
using UnityEngine;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        /// <summary>
        /// contains all sounds
        /// </summary>
        [SerializeField][Tooltip("Container for all sounds")]
        private System.Collections.Generic.List<Sound> sounds;

        private new void Awake()
        {
            foreach (Sound sound in sounds)
            {
                sound.Source = gameObject.AddComponent<AudioSource>();
                sound.Source.name = sound.Name;
                sound.Source.clip = sound.Clip;
                sound.Source.volume = sound.Volume;
                
            }
        }
        
        public void Play(string name)
        {
            Sound s = sounds.Find(sound => sound.Name == name);
            s.Source.Play();
        }
    }
}