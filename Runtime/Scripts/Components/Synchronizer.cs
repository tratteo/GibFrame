// Copyright (c) 2020 Matteo Beltrame

using System.Collections.Generic;
using GibFrame.Utils.Callbacks;
using UnityEngine;

namespace GibFrame.Components
{
    /// <summary>
    ///   Asynchronous -&gt; Synchronous
    /// </summary>
    public class Synchronizer : MonoBehaviour
    {
        #region Singleton

        private Queue<AbstractCallback> jobs = new Queue<AbstractCallback>();

        public static Synchronizer Instance { get; private set; } = null;

        /// <summary>
        ///   Enqueue a job to be performed, the job must have the following declaration:
        ///   <code>void F() </code>
        /// </summary>
        /// <param name="job"> </param>
        public void AddJob(AbstractCallback job)
        {
            jobs.Enqueue(job);
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
        }

        #endregion Singleton

        private void Update()
        {
            while (jobs.Count > 0)
            {
                jobs.Dequeue().Invoke();
            }
        }
    }
}