using System.Collections;
using Constants;
using Entity.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LoadDeathScene : MonoBehaviour
    {
        [SerializeField][Tooltip("White overlay that fades in")] private Image whitePanel;
        
        
        private void Awake()
        {
            whitePanel = gameObject.GetComponent<Image>();
            PlayerState.OnPlayerDeath += value => LoadScene();
        }

        private void LoadScene()
        {
            StartCoroutine( FadeThenLoad(2.5f, whitePanel));
        }
        
        private IEnumerator FadeThenLoad(float fadeDuration,  Image elementToFade)
        {
            Color color = elementToFade.color;
            float currentTime = 0f;
            
            
            while (currentTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(0f, 1f, currentTime / fadeDuration);
                color = new Color(color.r, color.g, color.b, alpha);
                elementToFade.color = color;
                currentTime += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(fadeDuration);
        
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
