using UnityEngine;

namespace Managers
{
    public class Singleton<T> : MonoBehaviour where T : class
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            if (Instance != null)
                Destroy(this.gameObject);
            else
                Instance = this as T;
        }
    }
}