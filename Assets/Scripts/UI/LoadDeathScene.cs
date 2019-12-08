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
        private Image _whitePanel;
        
        private void Awake()
        {
            _whitePanel = gameObject.GetComponent<Image>();
            PlayerState.OnPlayerDeath += value => LoadScene();
        }

        private void LoadScene()
        {
            StartCoroutine( FadeThenLoad(2.5f));
        }
        
        private IEnumerator FadeThenLoad(float fadeDuration)
        {
            float currentTime = 0f;
            Color color = _whitePanel.color;
            
            while (currentTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(0f, 1f, currentTime / fadeDuration);
                color = new Color(color.r, color.g, color.b, alpha);
                _whitePanel.color = color;
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
