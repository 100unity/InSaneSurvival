using System.Collections;
using UnityEngine;
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

        public void WaitForSeconds(float time, UnityAction onFinish)
        {
            StartCoroutine(WaitSeconds());

            IEnumerator WaitSeconds()
            {
                yield return new WaitForSeconds(time);
                onFinish?.Invoke();
            }
        }
    }
}