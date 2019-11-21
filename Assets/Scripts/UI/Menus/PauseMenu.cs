using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menus
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject pauseUI;
        [SerializeField] private bool freezeGameOnPause;
        [SerializeField] private GameObject optionsUI;
        [SerializeField] private GameObject backgroundDim;

        [Header("Buttons")] [SerializeField] private Button continueButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button optionsBackButton;

        public delegate void PauseDelegate(bool isPaused);

        public static PauseDelegate PauseEvent;

        private bool isPaused;

        private void Awake()
        {
            continueButton.onClick.AddListener(TogglePause);
            optionsButton.onClick.AddListener(() => Options(true));
            mainMenuButton.onClick.AddListener(GoToMainMenu);
            quitButton.onClick.AddListener(Quit);
            optionsBackButton.onClick.AddListener(() => Options(false));
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
            backgroundDim.SetActive(isPaused);
            pauseUI.SetActive(isPaused);
            Time.timeScale = isPaused && freezeGameOnPause ? 0 : 1;
            PauseEvent?.Invoke(isPaused);
        }

        private void GoToMainMenu()
        {
            TogglePause();
            SceneManager.LoadScene(Consts.MAIN_MENU_SCENE);
        }

        private void Options(bool show)
        {
            optionsUI.SetActive(show);
            pauseUI.SetActive(!show);
        }


        private void Quit()
        {
#if UNITY_EDITOR //Exits the play-mode
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}