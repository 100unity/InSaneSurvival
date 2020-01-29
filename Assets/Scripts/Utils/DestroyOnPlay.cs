using UnityEngine;

namespace Utils
{
    public class DestroyOnPlay : MonoBehaviour
    {
        private void Awake()
        {
            Destroy(gameObject);
        }
    }
}