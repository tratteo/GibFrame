//Copyright (c) matteo
//TickManager.cs - com.tratteo.gibframe

using System;
using System.Collections.Generic;
using System.Reflection;
using GibFrame.Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GibFrame.Performance
{
    public class TickManager : MonoSingleton<TickManager>
    {
        [Header("TickManager")]
        [SerializeField] private float defaultTickDelta;

        private List<TickedAgent> agents;
        private List<TickedAgent> next;

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
                    if (!agent.CustomDelta)
                    {
                        agent.TickDelta = defaultTickDelta;
                    }
                }
            }
        }

        public float GetDefaultDeltaTime() => defaultTickDelta;

        public void NotifyAgent(MonoBehaviour mono)
        {
            AnalyzeMono(mono);
        }

        protected override void Awake()
        {
            base.Awake();
            agents = new List<TickedAgent>();
            next = new List<TickedAgent>();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void CheckTickedAttribute(GameObject obj)
        {
            TickManager manager = FindObjectOfType<TickManager>();
            MonoBehaviour mono = obj.GetComponent<MonoBehaviour>();
            if (manager != null && mono != null)
            {
                manager.AnalyzeMono(mono);
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ScanTicked();
        }

        private void ScanTicked()
        {
            agents.Clear();
            MonoBehaviour[] behaviours = FindObjectsOfType<MonoBehaviour>(true);
            foreach (MonoBehaviour mono in behaviours)
            {
                AnalyzeMono(mono);
            }
        }

        private void AnalyzeMono(MonoBehaviour mono)
        {
            Type type = mono.GetType();
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                var attributes = method.GetCustomAttributes(typeof(TickedAttribute), true);
                foreach (object attr in attributes)
                {
                    TickedAttribute ticked = (TickedAttribute)attr;
                    bool customDelta = ticked.TickDelta >= 0 ? true : false;
                    TickedAgent agent = new TickedAgent(mono, method, customDelta ? ticked.TickDelta : defaultTickDelta, ticked.TickDisabled, customDelta);
                    agents.Add(agent);
                }
            }
        }

        private void Start()
        {
            ScanTicked();
        }

        private void Update()
        {
            next.Clear();
            foreach (TickedAgent agent in agents)
            {
                if (agent.Parent != null)
                {
                    next.Add(agent);
                    agent.Tick(Time.deltaTime);
                }
            }
            agents = new List<TickedAgent>(next);
        }
    }
}
