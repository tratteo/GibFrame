using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GibFrame
{
    public class Timer
    {
        private readonly MonoBehaviour context;
        private readonly List<AbstractCallback> callbacks;
        private readonly float delay;
        private readonly bool realtime;
        private bool running;
        private Coroutine Countdown;

        public Timer(MonoBehaviour context, float delay, bool realtime, bool start, params AbstractCallback[] callbacks)
        {
            this.callbacks = new List<AbstractCallback>();
            this.context = context;
            this.delay = delay;
            this.realtime = realtime;
            running = false;
            foreach (var callback in callbacks)
            {
                AddCallback(callback);
            }
            if (start)
            {
                Start();
            }
        }

        public void AddCallback(AbstractCallback callback) => callbacks.Add(callback);

        public bool RemoveCallback(AbstractCallback callback) => callbacks.Remove(callback);

        public void Start()
        {
            if (!running)
            {
                Countdown = context.StartCoroutine(Countdown_C());
            }
        }

        public void Stop()
        {
            if (running)
            {
                Countdown = null;
                context.StopCoroutine(Countdown);
            }
        }

        private IEnumerator Countdown_C()
        {
            running = true;
            if (realtime)
            {
                yield return new WaitForSecondsRealtime(delay);
            }
            else
            {
                yield return new WaitForSeconds(delay);
            }

            foreach (var callback in callbacks)
            {
                callback.Invoke();
            }
            running = false;
        }
    }
}