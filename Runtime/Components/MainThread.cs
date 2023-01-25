using System;
using System.Collections.Generic;
using UnityEngine;

namespace GibFrame
{
    /// <summary>
    ///   Safely execute methods that need to run on Unity's main thread from any other thread
    /// </summary>
    public class MainThread : MonoBehaviour
    {
        private static MainThread instance;
        private Queue<Action> updateJobs;
        private Queue<Action> fixedUpdateJobs;
        private Queue<Action> lateUpdateJobs;

        public static void InUpdate(Action action)
        {
            TryInitialize();
            lock (instance.updateJobs)
            {
                instance.updateJobs.Enqueue(action);
            }
        }

        public static void InFixedUpdate(Action action)
        {
            TryInitialize();
            lock (instance.fixedUpdateJobs)
            {
                instance.fixedUpdateJobs.Enqueue(action);
            }
        }

        public static void InLateUpdate(Action action)
        {
            TryInitialize();
            lock (instance.lateUpdateJobs)
            {
                instance.lateUpdateJobs.Enqueue(action);
            }
        }

        protected void Awake()
        {
            if (instance && instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }

            updateJobs = new Queue<Action>();
            fixedUpdateJobs = new Queue<Action>();
            lateUpdateJobs = new Queue<Action>();
        }

        private static void TryInitialize()
        {
            if (instance) return;
            var obj = new GameObject()
            {
                name = "MainThreadRunner",
                hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy | HideFlags.NotEditable
            };
            instance = obj.AddComponent<MainThread>();
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