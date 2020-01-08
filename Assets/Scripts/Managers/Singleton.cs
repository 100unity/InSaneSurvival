using Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class Singleton<T> : MonoBehaviour where T : class
    {
        /// <summary>
        /// Globally accessible Instance
        /// </summary>
        public static T Instance { get; private set; }

        /// <summary>
        /// Enables DontDestroyOnLoad and checks if there is already a Singleton of this type.
        /// <para>Add a specific Singleton to the "Script Execution Order" to load it before other Awakes get executed</para>
        /// </summary>
        protected virtual void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this as T;

            SceneManager.sceneUnloaded += GameSceneUnloaded;
        }

        /// <summary>
        /// Needed due to missing <see cref="UnityEngine.Object.DontDestroyOnLoad"/>
        /// </summary>
        private void GameSceneUnloaded(Scene scene)
        {
            if (scene.buildIndex != Consts.Scene.GAME) return;
            Instance = null;
            SceneManager.sceneUnloaded -= GameSceneUnloaded;
        }
    }
}