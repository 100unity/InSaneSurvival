using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [Tooltip("Button for starting the game")] [SerializeField]
        private Button playButton;

        [Tooltip("Button for opening the settings")] [SerializeField]
        private Button optionsButton;

        [Tooltip("Button for leaving the options")] [SerializeField]
        private Button optionsBackButton;

        [Tooltip("Button for exiting the game")] [SerializeField]
        private Button quitButton;

        [Tooltip("The content of the main menu")] [SerializeField]
        private GameObject mainMenuContent;

        [Tooltip("The content of the options menu")] [SerializeField]
        private GameObject optionsContent;

        /// <summary>
        /// Adds all listeners
        /// </summary>
        private void Awake()
        {
            playButton.onClick.AddListener(Play);
            optionsButton.onClick.AddListener(() => Options(true));
            optionsBackButton.onClick.AddListener(() => Options(false));
            quitButton.onClick.AddListener(GameManager.Quit);
        }

        /// <summary>
        /// Loads the GameScene
        /// </summary>
        private void Play() => SceneManager.LoadScene(Consts.GAME_SCENE);

        /// <summary>
        /// Shows/Hides the options
        /// </summary>
        /// <param name="show"></param>
        private void Options(bool show)
        {
            mainMenuContent.SetActive(!show);
            optionsContent.SetActive(show);
        }
    }
}