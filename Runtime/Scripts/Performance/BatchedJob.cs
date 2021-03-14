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

        public bool Dispatching { get; private set; } = false;

        public static BatchedJob Compose(int batch, YieldInstruction yieldInstruction)
        {
            GameObject obj = new GameObject
            {
                hideFlags = HideFlags.HideInHierarchy
            };
            BatchedJob job = obj.AddComponent<BatchedJob>();
            job.Setup(batch, yieldInstruction);
            return job;
        }

        public bool RemoveJob(AbstractCallback Op)
        {
            return Operations.Remove(Op);
        }

        public void AddJob(AbstractCallback Op)
        {
            Operations.Add(Op);
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

        private AbstractCallback Cycle()
        {
            AbstractCallback op = Operations.ElementAt(0);
            Operations.RemoveAt(0);
            Operations.Add(op);
            return op;
        }

        private IEnumerator Dispatcher_C()
        {
            int counter = 0;
            Dispatching = true;
            while (Operations.Count > 0)
            {
                AbstractCallback Op = Cycle();
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
