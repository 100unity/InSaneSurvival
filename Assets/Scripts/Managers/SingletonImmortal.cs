using UnityEngine;

namespace Managers
{
    public class SingletonImmortal<T> : MonoBehaviour where T : class
    {
        /// <summary>
        /// Globally accessible Instance, which doesn't get destroyed when switching Scenes
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
            
            DontDestroyOnLoad(gameObject);
        }
        
    }
}