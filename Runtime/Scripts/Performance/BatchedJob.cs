using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GibFrame.Utils.Callbacks;
using UnityEngine;

namespace GibFrame.Performance
{
    public class BatchedJob : MonoBehaviour
    {
        private List<AbstractCallback> Operations = new List<AbstractCallback>();
        private int batch;
        private YieldInstruction yieldInstruction;
        private bool oneTimeOps = false;
        private List<AbstractCallback> operationsBatch = new List<AbstractCallback>();

        private bool shouldRun = true;

        public bool Dispatching { get; private set; } = false;

        public static BatchedJob Compose(int batch, YieldInstruction yieldInstruction, bool oneTimeOps)
        {
            GameObject obj = new GameObject
            {
                hideFlags = HideFlags.HideInHierarchy
            };
            BatchedJob job = obj.AddComponent<BatchedJob>();
            job.Setup(batch, yieldInstruction, oneTimeOps);

            return job;
        }

        public int GetBatch() => batch;

        public void SetBatch(int batch)
        {
            this.batch = batch;
        }

        public bool RemoveJob(AbstractCallback Op)
        {
            return Operations.Remove(Op);
        }

        public void AddJob(AbstractCallback Op)
        {
            Operations.Add(Op);
        }

        public void ClearJobs()
        {
            Operations.Clear();
        }

        public void Dispose()
        {
            DestroyImmediate(gameObject);
        }

        public void Suspend()
        {
            shouldRun = false;
        }

        public void Dispatch()
        {
            shouldRun = true;
            if (!Dispatching)
            {
                StartCoroutine(Dispatcher_C());
            }
        }

        private void Setup(int batch, YieldInstruction yieldInstruction, bool oneTimeOps)
        {
            this.oneTimeOps = oneTimeOps;
            this.batch = batch;
            this.yieldInstruction = yieldInstruction;
        }

        private void PrepareBatch()
        {
            operationsBatch.Clear();
            for (int i = 0; i < batch; i++)
            {
                if (Operations.Count > 0)
                {
                    AbstractCallback current = Operations.ElementAt(0);
                    operationsBatch.Add(current);
                    if (oneTimeOps)
                    {
                        Operations.RemoveAt(0);
                    }
                }
                else
                {
                    return;
                }
            }
        }

        private IEnumerator Dispatcher_C()
        {
            Dispatching = true;
            while (shouldRun)
            {
                PrepareBatch();
                foreach (AbstractCallback callback in operationsBatch)
                {
                    callback?.Invoke();
                }
                yield return yieldInstruction;
            }
            Dispatching = false;
        }
    }
}
