using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button optionsButton;
        [SerializeField] private Button quitButton;
        [SerializeField] private Button optionsBackButton;

        [SerializeField] private GameObject mainMenuContent;
        [SerializeField] private GameObject optionsContent;

        private void Start()
        {
            playButton.onClick.AddListener(Play);
            optionsButton.onClick.AddListener(() => Options(true));
            optionsBackButton.onClick.AddListener(() => Options(false));
            quitButton.onClick.AddListener(Quit);
        }

        private void Play() => SceneManager.LoadScene(Consts.GAME_SCENE);

        private void Options(bool show)
        {
            mainMenuContent.SetActive(!show);
            optionsContent.SetActive(show);
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