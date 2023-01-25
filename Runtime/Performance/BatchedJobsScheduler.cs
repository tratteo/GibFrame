using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GibFrame.Performance
{
    public class BatchedJobsScheduler : MonoBehaviour
    {
        private readonly List<AbstractCallback> operations = new List<AbstractCallback>();
        private int batch;
        private YieldInstruction yieldInstruction;

        public bool Dispatching { get; private set; } = false;

        public static BatchedJobsScheduler Compose(int batch, YieldInstruction yieldInstruction)
        {
            var obj = new GameObject
            {
                hideFlags = HideFlags.HideInHierarchy,
                name = "BatchedJob"
            };
            var job = obj.AddComponent<BatchedJobsScheduler>();
            job.Setup(batch, yieldInstruction);

            return job;
        }

        public int GetBatch() => batch;

        public void SetBatch(int batch)
        {
            this.batch = batch;
        }

        public bool RemoveJob(AbstractCallback Op)
        {
            return operations.Remove(Op);
        }

        public void AddJob(params AbstractCallback[] Op)
        {
            operations.AddRange(Op);
            if (!Dispatching)
            {
                StartCoroutine(Dispatcher_C());
            }
        }

        public void ClearJobs()
        {
            operations.Clear();
        }

        public void Dispose()
        {
            DestroyImmediate(gameObject);
        }

        private void Setup(int batch, YieldInstruction yieldInstruction)
        {
            this.batch = batch;
            this.yieldInstruction = yieldInstruction;
        }

        private IEnumerator Dispatcher_C()
        {
            var counter = 0;
            Dispatching = true;
            while (operations.Count > 0)
            {
                var callback = operations.ElementAt(0);
                operations.RemoveAt(0);
                callback?.Invoke();
                counter++;
                operations.Add(callback);
                if (counter % batch == 0)
                {
                    counter = 0;
                    yield return yieldInstruction;
                }
            }
            Dispatching = false;
        }
    }
}