using UnityEngine;
using UnityEngine.Serialization;

namespace GameAudio
{
    [System.Serializable]
    public class Sound
    {
        private AudioSource _source;
        public AudioSource Source
        {
            get => _source;
            set => _source = value;
        }
        
        private string _name;
        public string Name
        {
            get => _name;
            set => _name = value;
        }
        
        private AudioClip _clip;
        public AudioClip Clip
        {
            get => _clip;
            set => _clip = value;
        }
        
        [Range(0f, 1f)]
        private float _volume;
        public float Volume
         {
            get => _volume;
            set => _volume = value;
        }
    }
}
