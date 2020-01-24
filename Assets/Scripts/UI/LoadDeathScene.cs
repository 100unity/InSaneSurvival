using System.Collections;
using Constants;
using Entity.Player;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LoadDeathScene : MonoBehaviour
    {
        [SerializeField] [Tooltip("The canvas for enabling/disabling")]
        private Canvas canvas;

        [SerializeField] [Tooltip("White overlay that fades in")]
        private Image whitePanel;


        private void Awake()
        {
            PlayerState.OnPlayerDeath += LoadScene;
        }

        private void LoadScene()
        {
            canvas.gameObject.SetActive(true);
            StartCoroutine(FadeThenLoad(1.5f, whitePanel));
        }

        // Fades in the white overlay and when finished loads "DeathScene"
        private IEnumerator FadeThenLoad(float fadeDuration, Image elementToFade)
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

            FadeOutAmbientSound();
            Load();
            PlayerState.OnPlayerDeath -= LoadScene;
        }
        
        // loads "DeathScene"
        private void Load()
        {
            SceneManager.LoadScene(Consts.Scene.DEATH);
            SceneManager.sceneLoaded += SceneLoadCompleted;
        }

        private void SceneLoadCompleted(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != Consts.Scene.DEATH) return;
            SceneManager.sceneLoaded -= SceneLoadCompleted;
        }

        private void FadeOutAmbientSound()
        {
            AudioManager.Instance.FadeOutPlaying(2);
        }
    }
}