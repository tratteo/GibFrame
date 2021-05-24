// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.Asynchronous.Await : AwaitableCoroutine.cs
//
// All Rights Reserved

using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;

namespace GibFrame.Asynchronous.Await
{
    public partial class AwaitableCoroutine<T> : INotifyCompletion
    {
        private Action Continuation;

        private bool isCompleted;

        public T Instruction { get; protected set; }

        public Enumerator Coroutine { get; private set; }

        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }
            protected set
            {
                isCompleted = value;

                if (value && Continuation != null)
                {
                    Continuation();
                    Continuation = null;
                }
            }
        }

        internal AwaitableCoroutine()
        {
        }

        internal AwaitableCoroutine(T instruction)
        {
            ProcessCoroutine(instruction);
        }

        public T GetResult()
        {
            return Instruction;
        }

        void INotifyCompletion.OnCompleted(Action continuation)
        {
            OnCompleted(continuation);
        }

        protected virtual void OnCompleted(Action continuation)
        {
            Continuation = continuation;
        }

        private void ProcessCoroutine(T instruction)
        {
            Instruction = instruction;
            Coroutine = new Enumerator(this);

            Awaiter.Instance.StartAwaiterCoroutine(this);
        }

        public class Enumerator : IEnumerator
        {
            private AwaitableCoroutine<T> parent;
            private IEnumerator nestedCoroutine;

            public object Current { get; private set; }

            internal Enumerator(AwaitableCoroutine<T> parent)
            {
                this.parent = parent;
                nestedCoroutine = parent.Instruction as IEnumerator;
            }

            bool IEnumerator.MoveNext()
            {
                if (nestedCoroutine != null)
                {
                    bool result = nestedCoroutine.MoveNext();
                    Current = nestedCoroutine.Current;
                    parent.IsCompleted = !result;

                    return result;
                }

                if (Current == null)
                {
                    Current = parent.Instruction;
                    return true;
                }

                parent.IsCompleted = true;
                return false;
            }

            void IEnumerator.Reset()
            {
                Current = null;
                parent.IsCompleted = false;
            }
        }
    }

    public class AwaiterCoroutineWaitForMainThread : AwaitableCoroutine<WaitForMainThread>
    {
        internal AwaiterCoroutineWaitForMainThread()
        {
            Instruction = default(WaitForMainThread);
        }

        protected override void OnCompleted(Action continuation)
        {
            base.OnCompleted(continuation);

            if (SynchronizationContext.Current != null)
            {
                IsCompleted = true;
            }
            else
            {
                Awaiter.Instance.SynchronizationContext.Post(state =>
                {
                    IsCompleted = true;
                }, null);
            }
        }
    }
}