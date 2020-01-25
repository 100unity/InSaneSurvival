using Constants;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameAudio
{
    public class PlayTheme : MonoBehaviour
    {
        [SerializeField][Tooltip("Name of the theme song audio")]
        private string themeSong;

        private void Awake()
        {
            SceneManager.sceneUnloaded += MenuSceneUnloaded;
        }

        private void Start()
        {
            AudioManager.Instance.FadeIn(themeSong,3);
        }

        private void MenuSceneUnloaded(Scene scene) 
        {
            if (scene.buildIndex != Consts.Scene.MAIN_MENU) return;
            AudioManager.Instance.FadeOut(themeSong,10);   
            SceneManager.sceneUnloaded -= MenuSceneUnloaded;
        }
    }
}
