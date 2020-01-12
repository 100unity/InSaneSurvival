using System.Security.Cryptography;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameAudio
{
    public class UIAudio : MonoBehaviour
    {
        [SerializeField][Tooltip("Name of the sound which is played on hover")]
        private string btnHoverSound;
        
        [SerializeField][Tooltip("Name of the sound which is played on click")]
        private string btnClickSound;
        
        /// <summary>
        /// Plays button hover sound
        /// </summary>
        public void PlayOnHover() {
            AudioManager.Instance.Play(btnHoverSound);
        }
        
        /// <summary>
        /// Plays button click sound
        /// </summary>
        public void PlayOnClick() {
            AudioManager.Instance.Play(btnClickSound);
        }
    }
}
