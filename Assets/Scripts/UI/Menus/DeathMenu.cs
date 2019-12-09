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
        }
    }
}