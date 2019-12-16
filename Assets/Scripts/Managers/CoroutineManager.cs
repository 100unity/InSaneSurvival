using System.Collections;
using UnityEngine.Events;

namespace Managers
{
    public class CoroutineManager : Singleton<CoroutineManager>
    {
        public void WaitForOneFrame(UnityAction onFinish)
        {
            StartCoroutine(WaitFrame());

            IEnumerator WaitFrame()
            {
                yield return null;
                onFinish?.Invoke();
            }
        }
    }
}