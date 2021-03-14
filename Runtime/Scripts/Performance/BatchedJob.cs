using System.Collections;
using System.Collections.Generic;
using GibFrame.Utils.Callbacks;
using UnityEngine;

namespace GibFrame.Performance
{
    public class BatchedJob : MonoBehaviour
    {
        private Queue<AbstractCallback> Operations = new Queue<AbstractCallback>();
        private int batch;
        private YieldInstruction yieldInstruction;

        public bool Dispatching { get; private set; } = false;

        public static BatchedJob Compose(int batch, YieldInstruction yieldInstruction)
        {
            GameObject obj = new GameObject();
            obj.hideFlags = HideFlags.HideInHierarchy;
            BatchedJob job = obj.AddComponent<BatchedJob>();
            job.Setup(batch, yieldInstruction);
            return job;
        }

        public void AddJob(AbstractCallback Op)
        {
            Operations.Enqueue(Op);
            if (!Dispatching)
            {
                StartCoroutine(Dispatcher_C());
            }
        }

        public void ClearJobs()
        {
            Operations.Clear();
        }

        private void Setup(int batch, YieldInstruction yieldInstruction)
        {
            this.batch = batch;
            this.yieldInstruction = yieldInstruction;
        }

        private IEnumerator Dispatcher_C()
        {
            int counter = 0;
            Dispatching = true;
            while (Operations.Count > 0)
            {
                AbstractCallback Op = Operations.Dequeue();
                Operations.Enqueue(Op);
                Op?.Invoke();
                counter++;
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
