using System;
using System.Collections.Generic;
using UnityEngine;

namespace GibFrame
{
    public class Clock : AbstractClock
    {
        private List<AbstractCallback> callbacks;

        public Clock(MonoBehaviour context, float fixedRate, bool startNow, params AbstractCallback[] callbacks) : base(context, fixedRate, startNow)
        {
            InitializeCallbacks(callbacks);
        }

        public Clock(MonoBehaviour context, Vector2 rateRange, bool startNow, params AbstractCallback[] callbacks) : base(context, rateRange, startNow)
        {
            InitializeCallbacks(callbacks);
        }

        public Clock(MonoBehaviour context, Func<int, float> scaleFunc, bool startNow, params AbstractCallback[] callbacks) : base(context, scaleFunc, startNow)
        {
            InitializeCallbacks(callbacks);
        }

        public void AddCallback<T>(T callback) where T : AbstractCallback
        {
            callbacks.Add(callback);
        }

        public void RemoveCallback<T>(T callback) where T : AbstractCallback
        {
            callbacks.Remove(callback);
        }

        protected override void Callback()
        {
            foreach (var callback in callbacks)
            {
                callback.Invoke();
            }
        }

        private void InitializeCallbacks(params AbstractCallback[] callbacks)
        {
            this.callbacks = new List<AbstractCallback>();
            foreach (var callback in callbacks)
            {
                AddCallback(callback);
            }
        }
    }
}