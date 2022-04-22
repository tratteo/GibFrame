using System;
using System.Collections.Generic;

namespace GibFrame
{
    /// <summary>
    ///   Safely execute methods that needs to run on Unity's main thread from any other thread
    /// </summary>
    public class ThreadSafe : MonoSingleton<ThreadSafe>
    {
        private Queue<Action> updateJobs;
        private Queue<Action> fixedUpdateJobs;
        private Queue<Action> lateUpdateJobs;

        public static void InUpdate(Action action) => Instance.updateJobs.Enqueue(action);

        public static void InFixedUpdate(Action action) => Instance.fixedUpdateJobs.Enqueue(action);

        public static void InLateUpdate(Action action) => Instance.lateUpdateJobs.Enqueue(action);

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
                updateJobs.Dequeue().Invoke();
            }
        }

        private void FixedUpdate()
        {
            if (fixedUpdateJobs.Count > 0)
            {
                fixedUpdateJobs.Dequeue().Invoke();
            }
        }

        private void LateUpdate()
        {
            if (lateUpdateJobs.Count > 0)
            {
                lateUpdateJobs.Dequeue().Invoke();
            }
        }
    }
}
