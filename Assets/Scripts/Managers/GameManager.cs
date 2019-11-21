using UI.Menus;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private PauseMenu pauseMenu;

        public void TogglePause() => pauseMenu.TogglePause();
    }
}