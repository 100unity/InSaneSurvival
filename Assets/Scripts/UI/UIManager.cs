using System;
using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private StatBar healthBar;
        [SerializeField] private StatBar saturationBar;
        [SerializeField] private StatBar hydrationBar;
        [SerializeField] private FadeElement whiteLight;
        private void Awake()
        {
            PlayerState.OnPlayerHealthUpdated += value => healthBar.UpdateBar(value);
            PlayerState.OnPlayerHydrationUpdated += value => hydrationBar.UpdateBar(value);
            PlayerState.OnPlayerSaturationUpdated += value => saturationBar.UpdateBar(value);
            PlayerState.OnPlayerDeath += value => LoadDeathScene();
        }

        private void LoadDeathScene()
        {
            StartCoroutine( WaitThenLoad(2.5f));
        }
        
        private IEnumerator WaitThenLoad(float fadeDuration)
        {
            whiteLight.FadeIn(fadeDuration);
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