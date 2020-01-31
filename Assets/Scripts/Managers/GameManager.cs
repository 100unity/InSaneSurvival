using UI.Menus;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [Tooltip("The PauseMenu reference")] [SerializeField]
        private PauseMenu pauseMenu;

        /// <summary>
        /// Shows/Hides the pause menu
        /// </summary>
        public void TogglePause()
        {
            pauseMenu.TogglePause();
            CursorManager.Instance.SetCursorToDefault();
        }


        /// <summary>
        /// Quits the game (Editor and Standalone)
        /// </summary>
        public static void Quit()
        {
#if UNITY_EDITOR //Exits the play-mode
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}