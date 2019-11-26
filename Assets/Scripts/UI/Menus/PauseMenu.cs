using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menus
{
    public class PauseMenu : MonoBehaviour
    {
        [Tooltip("The content of the pause menu")] [SerializeField]
        private GameObject pauseContent;

        [Tooltip("The content of the options menu")] [SerializeField]
        private GameObject optionsContent;

        [Tooltip("The background dim (black, semi-transparent image)")] [SerializeField]
        private GameObject backgroundDim;

        [Tooltip("Whether the game should be frozen/stopped onPause")] [SerializeField]
        private bool freezeGameOnPause;

        [Header("Buttons")] [Tooltip("The button for continuing the game")] [SerializeField]
        private Button continueButton;

        [Tooltip("The button for opening the options")] [SerializeField]
        private Button optionsButton;

        [Tooltip("The button for leaving the options")] [SerializeField]
        private Button optionsBackButton;

        [Tooltip("The button for going back to the main menu")] [SerializeField]
        private Button mainMenuButton;

        [Tooltip("The button for exiting the game")] [SerializeField]
        private Button quitButton;


        public delegate void PauseDelegate(bool isPaused);

        /// <summary>
        /// Will be invoked when the game gets paused
        /// </summary>
        public static PauseDelegate OnPause;


        private bool _isPaused;

        /// <summary>
        /// Adds all listeners
        /// </summary>
        private void Awake()
        {
            continueButton.onClick.AddListener(TogglePause);
            optionsButton.onClick.AddListener(() => Options(true));
            mainMenuButton.onClick.AddListener(GoToMainMenu);
            quitButton.onClick.AddListener(GameManager.Quit);
            optionsBackButton.onClick.AddListener(() => Options(false));
        }

        /// <summary>
        /// Shows/Hides the pause menu
        /// </summary>
        public void TogglePause()
        {
            _isPaused = !_isPaused;
            backgroundDim.SetActive(_isPaused);
            pauseContent.SetActive(_isPaused);
            if(!_isPaused)
                optionsContent.SetActive(false);
            Time.timeScale = _isPaused && freezeGameOnPause ? 0 : 1;
            OnPause?.Invoke(_isPaused);
        }

        /// <summary>
        /// Loads the MainMenuScene
        /// </summary>
        private void GoToMainMenu()
        {
            SceneManager.LoadScene(Consts.MAIN_MENU_SCENE);
            SceneManager.sceneLoaded += SceneLoadCompleted;
        }

        /// <summary>
        /// This will wait until the MainMenuScene is loaded before hiding the PauseMenu
        /// </summary>
        /// <param name="scene">The loaded scene</param>
        /// <param name="mode">The loading mode</param>
        private void SceneLoadCompleted(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex != Consts.MAIN_MENU_SCENE) return;
            SceneManager.sceneLoaded -= SceneLoadCompleted;
            TogglePause();
        }

        /// <summary>
        /// Shows/Hides the options
        /// </summary>
        /// <param name="show"></param>
        private void Options(bool show)
        {
            optionsContent.SetActive(show);
            pauseContent.SetActive(!show);
        }
    }
}