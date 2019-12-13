using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Utils
{
    public class FadeElement : MonoBehaviour
    {
        
        [SerializeField][Tooltip("UI Element to fade in or out")] private Image elementToFade;
        
        /// <summary>
        /// Fades in an UI Element
        /// </summary>
        /// <param name="duration">The duration of the fading animation in seconds</param>
        public void FadeIn(float duration)
        {
            StartCoroutine(Fade(duration,0,1));
        }

        /// <summary>
        /// Fades out an UI Element
        /// </summary>
        /// <param name="duration">The duration of the fading animation in seconds</param>
        public void FadeOut(float duration)
        {
            StartCoroutine(Fade(duration, 1,0));
        }

        private IEnumerator Fade(float duration, float startAlpha, float endAlpha)
        {
            float currentTime = 0f;
            Color color = elementToFade.color;

            while (currentTime < duration)
            {
                float alpha = Mathf.Lerp(startAlpha, endAlpha, currentTime / duration);
                color = new Color(color.r, color.g, color.b, alpha);
                elementToFade.color = color;
                currentTime += Time.deltaTime;
                yield return null;
            }
            if (endAlpha <= 0) gameObject.SetActive(false);
        }
    }

}