using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DeathScene : MonoBehaviour
    {
        [SerializeField][Tooltip("White overlay that fades in the scene")] private Image whitePanel;
        [SerializeField][Tooltip("Model of the players character")] private Animator playerCharacterAnimator;
        

        // Start is called before the first frame update

        private void Start()
        {
            StartCoroutine( FadeOut(2, whitePanel));
            playerCharacterAnimator.SetBool ("isDead", true);
        }

        // fade out UI Objects
        private IEnumerator FadeOut(float fadeDuration, Image elementToFade)
        {
            Color color = elementToFade.color;
            float currentTime = 0f;

            while (currentTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
                color = new Color(color.r, color.g, color.b, alpha);
                elementToFade.color = color;
                currentTime += Time.deltaTime;
                yield return null;
            }
            elementToFade.enabled = false;
        }
    }
}
