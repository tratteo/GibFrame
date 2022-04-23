using System;
using System.Collections.Generic;

namespace GibFrame
{
    /// <summary>
    ///   Safely execute methods that needs to run on Unity's main thread from any other thread
    /// </summary>
    public class MainThread : MonoSingleton<MainThread>
    {
        private Queue<Action> updateJobs;
        private Queue<Action> fixedUpdateJobs;
        private Queue<Action> lateUpdateJobs;

        public static void InUpdate(Action action)
        {
            lock (Instance.updateJobs)
            {
                Instance.updateJobs.Enqueue(action);
            }
        }

        public static void InFixedUpdate(Action action)
        {
            lock (Instance.fixedUpdateJobs)
            {
                Instance.fixedUpdateJobs.Enqueue(action);
            }
        }

        public static void InLateUpdate(Action action)
        {
            lock (Instance.lateUpdateJobs)
            {
                Instance.lateUpdateJobs.Enqueue(action);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            updateJobs = new Queue<Action>();
            fixedUpdateJobs = new Queue<Action>();
            lateUpdateJobs = new Queue<Action>();
        }

        private void Update()
        {
            if (updateJobs.Count > 0)
            {
                lock (updateJobs)
                {
                    updateJobs.Dequeue().Invoke();
                }
            }
        }

        private void FixedUpdate()
        {
            if (fixedUpdateJobs.Count > 0)
            {
                lock (fixedUpdateJobs)
                {
                    fixedUpdateJobs.Dequeue().Invoke();
                }
            }
        }

        private void LateUpdate()
        {
            if (lateUpdateJobs.Count > 0)
            {
                lock (lateUpdateJobs)
                {
                    lateUpdateJobs.Dequeue().Invoke();
                }
            }
        }
    }
}
