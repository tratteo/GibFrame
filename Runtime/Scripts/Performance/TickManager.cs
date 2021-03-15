//Copyright (c) matteo
//TickManager.cs - com.tratteo.gibframe

using System.Collections.Generic;
using GibFrame.Patterns;
using GibFrame.Utils;
using GibFrame.Utils.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GibFrame.Performance
{
    public class TickManager : MonoSingleton<TickManager>
    {
        [Header("TickManager")]
        [SerializeField] private float defaultTickDelta;
        [Tooltip("Set the maximum amount of tickable objects per frame. Set to -1 to allow all objects to tick in the same frame")]
        [SerializeField] private int batchCount = -1;
        private List<TickedAgent> agents = new List<TickedAgent>();
        private List<TickedAgent> destroyed = new List<TickedAgent>();
        private BatchedJob tickBatchJob;

        public static GameObject Instantiate<T>(GameObject original) where T : Component
        {
            GameObject res = UnityEngine.Object.Instantiate(original);
            CheckTickedAttribute(res);
            return res;
        }

        public static GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation)
        {
            GameObject res = UnityEngine.Object.Instantiate(original, position, rotation);
            CheckTickedAttribute(res);
            return res;
        }

        public static GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject res = UnityEngine.Object.Instantiate(original, position, rotation, parent);
            CheckTickedAttribute(res);
            return res;
        }

        public static GameObject Instantiate(GameObject original, Transform parent, bool instantiateInWorldSpace)
        {
            GameObject res = UnityEngine.Object.Instantiate(original, parent, instantiateInWorldSpace);
            CheckTickedAttribute(res);
            return res;
        }

        public void SetDefaultDeltaTime(float deltaTime)
        {
            defaultTickDelta = deltaTime;
            if (agents != null)
            {
                foreach (TickedAgent agent in agents)
                {
                    if (!agent.Parameters.CustomDelta)
                    {
                        agent.Parameters.TickDelta = defaultTickDelta;
                    }
                }
            }
        }

        public float GetDefaultDeltaTime() => defaultTickDelta;

        public bool IsBatched() => batchCount > 0;

        public void SetBatchCount(int batch)
        {
            batchCount = batch;
            tickBatchJob.SetBatch(batch);
        }

        protected override void Awake()
        {
            base.Awake();
            destroyed = new List<TickedAgent>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            tickBatchJob = BatchedJob.Compose(batchCount, new WaitForEndOfFrame(), true);
            tickBatchJob.Dispatch();
        }

        private static void CheckTickedAttribute(GameObject res)
        {
            if (Instance == null) return;
            ITickable tickable;
            if ((tickable = res.GetComponent<ITickable>()) != null)
            {
                Instance.agents.Add(new TickedAgent(tickable, Instance.GetDefaultDeltaTime()));
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Rescan();
        }

        private void Rescan()
        {
            agents.Clear();
            agents.AddRange(UnityUtils.GetInterfacesOfType<ITickable>().ConvertAll((t) => new TickedAgent(t, GetDefaultDeltaTime())));
        }

        private void Start()
        {
            Rescan();
        }

        private void TickOp(TickedAgent agent)
        {
            if (agent.Parent != null)
            {
                agent.Tick();
            }
        }

        private void Update()
        {
            destroyed.Clear();
            if (IsBatched())
            {
                foreach (TickedAgent agent in agents)
                {
                    if (agent.Parent != null)
                    {
                        agent.Step(Time.deltaTime);
                        if (agent.CanTick())
                        {
                            tickBatchJob.AddJob(new Callback<TickedAgent>(TickOp, agent));
                            agent.Reset();
                        }
                    }
                    else
                    {
                        destroyed.Add(agent);
                    }
                }
            }
            else
            {
                foreach (TickedAgent agent in agents)
                {
                    if (agent.Parent != null)
                    {
                        agent.Step(Time.deltaTime);
                        if (agent.CanTick())
                        {
                            TickOp(agent);
                        }
                    }
                    else
                    {
                        destroyed.Add(agent);
                    }
                }
            }

            foreach (TickedAgent agent in destroyed)
            {
                agents.Remove(agent);
            }
        }
    }
}
