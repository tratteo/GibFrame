// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.Asynchronous.Await : AwaiterCoroutineExtension.cs
//
// All Rights Reserved

using System.Collections;
using UnityEngine;

namespace GibFrame.Asynchronous.Await
{
    public struct WaitForNextFrame { }

    public struct WaitForMainThread { }

    public static class AwaiterCoroutineExtension
    {
        public static AwaitableCoroutine<IEnumerator> GetAwaiter(this IEnumerator coroutine)
        {
            return new AwaitableCoroutine<IEnumerator>(coroutine);
        }

        public static AwaitableCoroutine<WaitForNextFrame> GetAwaiter(this WaitForNextFrame waitForNextFrame)
        {
            return new AwaitableCoroutine<WaitForNextFrame>(waitForNextFrame);
        }

        public static AwaitableCoroutine<WaitForSeconds> GetAwaiter(this WaitForSeconds waitForSeconds)
        {
            return new AwaitableCoroutine<WaitForSeconds>(waitForSeconds);
        }

        public static AwaitableCoroutine<WaitForSecondsRealtime> GetAwaiter(this WaitForSecondsRealtime waitForSecondsRealtime)
        {
            return new AwaitableCoroutine<WaitForSecondsRealtime>(waitForSecondsRealtime);
        }

        public static AwaitableCoroutine<WaitForEndOfFrame> GetAwaiter(this WaitForEndOfFrame waitForEndOfFrame)
        {
            return new AwaitableCoroutine<WaitForEndOfFrame>(waitForEndOfFrame);
        }

        public static AwaitableCoroutine<WaitForFixedUpdate> GetAwaiter(this WaitForFixedUpdate waitForFixedUpdate)
        {
            return new AwaitableCoroutine<WaitForFixedUpdate>(waitForFixedUpdate);
        }

        public static AwaitableCoroutine<WaitUntil> GetAwaiter(this WaitUntil waitUntil)
        {
            return new AwaitableCoroutine<WaitUntil>(waitUntil);
        }

        public static AwaitableCoroutine<WaitWhile> GetAwaiter(this WaitWhile waitWhile)
        {
            return new AwaitableCoroutine<WaitWhile>(waitWhile);
        }

        public static AwaitableCoroutine<AsyncOperation> GetAwaiter(this AsyncOperation asyncOperation)
        {
            return new AwaitableCoroutine<AsyncOperation>(asyncOperation);
        }

        public static AwaitableCoroutine<CustomYieldInstruction> GetAwaiter(this CustomYieldInstruction customYieldInstruction)
        {
            return new AwaitableCoroutine<CustomYieldInstruction>(customYieldInstruction);
        }

        public static AwaiterCoroutineWaitForMainThread GetAwaiter(this WaitForMainThread waitForMainThread)
        {
            return new AwaiterCoroutineWaitForMainThread();
        }
    }
}