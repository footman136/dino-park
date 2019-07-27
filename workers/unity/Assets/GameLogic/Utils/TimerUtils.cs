using System; // Action
using System.Collections; // IEnumerator
using UnityEngine; // WaitForSeconds

namespace GameLogic.Utils
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
