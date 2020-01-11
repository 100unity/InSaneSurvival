using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class CoroutineManager : Singleton<CoroutineManager>
    {
        /// <summary>
        /// Waits the given time, then executes the given UnityAction.
        /// </summary>
        /// <param name="time">Time to wait</param>
        /// 
        /// <param name="onFinish">Action to be executed after one frame</param>
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