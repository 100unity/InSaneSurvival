using System;
using Managers;
using UnityEngine;

namespace GameAudio
{
    public class PlayTheme : MonoBehaviour
    {
        [SerializeField][Tooltip("Name of the theme song audio")]
        private string themeSong;
        
        void Start()
        {
        AudioManager.Instance.FadeIn(themeSong,3);
        }
    }
}
