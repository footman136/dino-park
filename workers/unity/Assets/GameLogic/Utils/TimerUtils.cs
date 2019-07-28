using System;
using System.Collections;
using UnityEngine;
// Action
// IEnumerator

// WaitForSeconds

namespace Assets.Gamelogic.Utils
{
    public static class TimerUtils
    {
        public static IEnumerator WaitAndPerform(float bufferTime, Action action)
        {
            yield return new WaitForSeconds(bufferTime);
            action();
        }

        public static IEnumerator CallRepeatedly(float interval, Action action)
        {
            while (true)
            {
                yield return new WaitForSeconds(interval);
                action();
            }
        }
    }
}
