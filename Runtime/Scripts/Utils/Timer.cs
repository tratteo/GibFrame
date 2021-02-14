// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.Utils : Timer.cs
//
// All Rights Reserved

using System;
using System.Collections;
using System.Collections.Generic;
using GibFrame.Utils.Callbacks;
using UnityEngine;

namespace GibFrame.Utils
{
    public class Timer
    {
        private float delay;
        private bool realtime;
        private bool running;
        private MonoBehaviour context;
        private Coroutine Countdown;

        private List<AbstractCallback> callbacks;

        public Timer(MonoBehaviour context, float delay, bool realtime, bool start, params AbstractCallback[] callbacks)
        {
            this.callbacks = new List<AbstractCallback>();
            this.context = context;
            this.delay = delay;
            this.realtime = realtime;
            running = false;
            foreach (AbstractCallback callback in callbacks)
            {
                AddCallback(callback);
            }
            if (start)
            {
                Start();
            }
        }

        public void AddCallback<T>(T callback) where T : AbstractCallback
        {
            callbacks.Add(callback);
        }

        public bool RemoveCallback<T>(T callback) where T : AbstractCallback
        {
            return callbacks.Remove(callback);
        }

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

            foreach (AbstractCallback callback in callbacks)
            {
                callback.Invoke();
            }
            running = false;
        }
    }
}