using Constants;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [Tooltip("Button for continuing an saved game")] [SerializeField]
        private GameObject continueButton;

        [Tooltip("Default filename of the savegame to load with continue button")] [SerializeField]
        private string saveGameFile;

        [Tooltip("Button for starting a new game")] [SerializeField]
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
            CheckForSaveGame();
            playButton.onClick.AddListener(Play);
            optionsButton.onClick.AddListener(() => Options(true));
            optionsBackButton.onClick.AddListener(() => Options(false));
            quitButton.onClick.AddListener(GameManager.Quit);
        }

        private void CheckForSaveGame()
        {
            string pathToFile = @"" + Application.persistentDataPath + "/" + saveGameFile + ".json";

            if (System.IO.File.Exists(pathToFile))
            {
                continueButton.GetComponent<Button>().onClick.AddListener(Continue);
                continueButton.SetActive(true);
            }
        }

        /// <summary>
        /// Loads the GameScene in a previously saved state
        /// </summary>
        private void Continue()
        {
            SceneManager.LoadScene(Consts.Scene.GAME);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// Loads the GameScene as a new game
        /// </summary>
        private void Play() => SceneManager.LoadScene(Consts.Scene.GAME);

        /// <summary>
        /// Shows/Hides the options
        /// </summary>
        /// <param name="show"></param>
        private void Options(bool show)
        {
            mainMenuContent.SetActive(!show);
            optionsContent.SetActive(show);
        }

        /// <summary>
        /// Waits for GameScene to be loaded, then loads Save
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SaveManager.Load(saveGameFile);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}