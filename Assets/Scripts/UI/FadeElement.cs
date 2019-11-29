using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace UI
{
    public class FadeElement : MonoBehaviour
    {
        private Image _element;

        private void Awake()
        {
            _element = gameObject.GetComponent<Image>();
        }

        public void FadeIn(float duration)
        {
            StartCoroutine(FadeInCr(duration));
        }
    
        private IEnumerator FadeInCr(float duration)
        {
            float currentTime = 0f;

            while (currentTime < duration)
            {
                float alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
                _element.color = new Color(_element.color.r, _element.color.g, _element.color.b, alpha);
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
        
        public void FadeOut(float duration)
        {
            StartCoroutine(FadeOutCr(duration));
        }
    
        private IEnumerator FadeOutCr(float duration)
        {
            float currentTime = 0f;

            while (currentTime < duration)
            {
                float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
                _element.color = new Color(_element.color.r, _element.color.g, _element.color.b, alpha);
                currentTime += Time.deltaTime;
                yield return null;
            }
        }
        
        private void GoToDeathScene()
        {
            SceneManager.LoadScene(Consts.Scene.DEATH);
            SceneManager.sceneLoaded += SceneLoadCompleted;
        }
        
        private void SceneLoadCompleted(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != Consts.Scene.DEATH) return;
            SceneManager.sceneLoaded -= SceneLoadCompleted;
        }
    }

}