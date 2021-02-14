// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.Asynchronous.Await : Awaiter.cs
//
// All Rights Reserved

using System.Threading;
using UnityEngine;

namespace GibFrame.Asynchronous.Await
{
    internal class Awaiter : MonoBehaviour
    {
        private static Awaiter instance;

        internal static Awaiter Instance
        {
            get
            {
                Install();
                return instance;
            }
        }

        internal SynchronizationContext SynchronizationContext { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        internal static void Install()
        {
            if (instance == null)
            {
                GameObject obj = new GameObject("Awaiter");
                instance = obj.AddComponent<Awaiter>();
                obj.hideFlags = HideFlags.HideAndDontSave;
            }
        }

        internal void StartAwaiterCoroutine<T>(AwaitableCoroutine<T> awaiterCoroutine)
        {
            StartCoroutine(awaiterCoroutine.Coroutine);
        }

        internal void StopAwaiterCoroutine<T>(AwaitableCoroutine<T> awaiterCoroutine)
        {
            StopCoroutine(awaiterCoroutine.Coroutine);
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;

            DontDestroyOnLoad(instance);
            SynchronizationContext = SynchronizationContext.Current;
        }
    }
}