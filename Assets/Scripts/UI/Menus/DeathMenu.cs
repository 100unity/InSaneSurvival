using Constants;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menus
{
    public class DeathMenu : MonoBehaviour
    {
        [Tooltip("The button for going back to the main menu")] [SerializeField]
        private Button mainMenuButton;

        /// <summary>
        /// Adds all listeners
        /// </summary>
        private void Awake()
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
        
        /// <summary>
        /// Loads the MainMenuScene
        /// </summary>
        private void GoToMainMenu()
        {
            SceneManager.LoadScene(Consts.Scene.MAIN_MENU);
            SceneManager.sceneLoaded += SceneLoadCompleted;
        }

        /// <summary>
        /// This will wait until the MainMenuScene is loaded before hiding the PauseMenu
        /// </summary>
        /// <param name="scene">The loaded scene</param>
        /// <param name="mode">The loading mode</param>
        private void SceneLoadCompleted(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != Consts.Scene.MAIN_MENU) return;
            SceneManager.sceneLoaded -= SceneLoadCompleted;
        }
    }
}