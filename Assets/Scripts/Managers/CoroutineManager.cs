using System;
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

        /// <summary>
        /// Waits for the given predicate, then executes the given UnityAction.
        /// </summary>
        /// <param name="predicate">The predicates that needs to be true</param>
        /// <param name="onFinish">Action to be executed afterwards</param>
        public void WaitUntil(Func<bool> predicate, UnityAction onFinish)
        {
            StartCoroutine(WaitUntil());

            IEnumerator WaitUntil()
            {
                yield return new WaitUntil(predicate);
                onFinish?.Invoke();
            }
        }

        /// <summary>
        /// Waits for the given frames, then executes the given UnityAction.
        /// </summary>
        /// <param name="frames">The number of frames</param>
        /// <param name="onFinish">Action to be executed afterwards</param>
        public void WaitForFrames(int frames, UnityAction onFinish)
        {
            StartCoroutine(WaitFrames());

            IEnumerator WaitFrames()
            {
                for (int i = 0; i < frames; i++)
                    yield return null;

                onFinish?.Invoke();
            }
        }
    }
}