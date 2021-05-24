// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.Performance : CommonUpdateManager.cs
//
// All Rights Reserved

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GibFrame.Performance
{
    public class CommonUpdateManager : MonoSingleton<CommonUpdateManager>
    {
        private readonly List<ICommonFixedUpdate> commonFixedUpdates = new List<ICommonFixedUpdate>();

        private readonly List<ICommonLateUpdate> commonLateUpdates = new List<ICommonLateUpdate>();

        private readonly List<ICommonUpdate> commonUpdates = new List<ICommonUpdate>();

        [Header("Preferences")]
        [Tooltip("Scan for all MonoBehaviours on scene loading")]
        [SerializeField] private bool scanOnSceneLoading = false;

        [Tooltip("Clear all the subscriptions when rescanning")]
        [SerializeField] private bool clearOnRescan = false;

        [Tooltip("Whether should disabled gameobjects be updated")]
        [SerializeField] private bool updateDisabled = false;

        /// <summary>
        ///   Returns the number of successful registrations for this object. Returns <strong> -1 </strong> if the instance of the
        ///   CommonUpdateManager is <strong> <c> null </c></strong>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="update"> </param>
        /// <returns> </returns>
        public static int Register<T>(T update) where T : class
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<CommonUpdateManager>();
#if !GIB_NO_COMM_RUNTIME_INSTANTIATE
                if (Instance == null)
                {
                    GameObject obj = new GameObject()
                    {
                        name = "CommonUpdateManagerRuntime"
                    };
                    Instance = obj.AddComponent<CommonUpdateManager>();
                }
#endif
            }
            if (Instance != null)
            {
                int registrations = 0;
                if (update is ICommonUpdate commonUpdate)
                {
                    if (!Instance.commonUpdates.Contains(commonUpdate))
                    {
                        Instance.commonUpdates.Add(commonUpdate);
                        registrations++;
                    }
                }
                if (update is ICommonFixedUpdate fixedUpdate)
                {
                    if (!Instance.commonFixedUpdates.Contains(fixedUpdate))
                    {
                        Instance.commonFixedUpdates.Add(fixedUpdate);
                        registrations++;
                    }
                }
                if (update is ICommonLateUpdate lateUpdate)
                {
                    if (!Instance.commonLateUpdates.Contains(lateUpdate))
                    {
                        Instance.commonLateUpdates.Add(lateUpdate);
                        registrations++;
                    }
                }
                return registrations;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Common updating is being used but the Instance of the CommonUpdateManager is null. Either add a CommonUpdateManager in the scene or enable auto runtime instantiation in GibFrame settings");
                return -1;
            }
        }

        /// <summary>
        ///   Returns the number of successful unregistrations for this object. Returns <strong> -1 </strong> if the instance of the
        ///   CommonUpdateManager is <strong> <c> null </c></strong>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="update"> </param>
        /// <returns> </returns>
        public static int Unregister<T>(T update) where T : class
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<CommonUpdateManager>();
            }
            if (Instance != null)
            {
                int unregistrations = 0;
                if (update is ICommonUpdate commonUpdate)
                {
                    unregistrations += Instance.commonUpdates.Remove(commonUpdate) ? 1 : 0;
                }
                if (update is ICommonFixedUpdate fixedUpdate)
                {
                    unregistrations += Instance.commonFixedUpdates.Remove(fixedUpdate) ? 1 : 0;
                }
                if (update is ICommonLateUpdate lateUpdate)
                {
                    unregistrations += Instance.commonLateUpdates.Remove(lateUpdate) ? 1 : 0;
                }
                return unregistrations;
            }
            else
            {
                //UnityEngine.Debug.LogWarning("Common updating is being used but the Instance of the CommonUpdateManager is null. Be sure that a CommonUpdateManager is present in the scene");
                return -1;
            }
        }

        public void Rescan()
        {
            if (clearOnRescan)
            {
                commonUpdates.Clear();
                commonFixedUpdates.Clear();
                commonLateUpdates.Clear();
            }

            MonoBehaviour[] monos = FindObjectsOfType<MonoBehaviour>();
            monos.ForEach((m) => Register(m));
        }

        protected override void Awake()
        {
            base.Awake();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private bool CanUpdate<T>(T update)
        {
            if (update is MonoBehaviour mono)
            {
                if (!mono) return false;
                return mono.enabled && (updateDisabled || mono.gameObject.activeSelf);
            }
            return update != null;
        }

        private bool IsValid<T>(T update)
        {
            if (update is MonoBehaviour mono)
            {
                return mono;
            }
            return update != null;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scanOnSceneLoading)
            {
                Rescan();
            }
        }

#if !GIB_NO_COMMFIXEDUPDATE

        private void FixedUpdate()
        {
            List<ICommonFixedUpdate> unmutable = new List<ICommonFixedUpdate>(commonFixedUpdates);
            foreach (ICommonFixedUpdate update in unmutable)
            {
                if (CanUpdate(update))
                {
                    update.CommonFixedUpdate(Time.fixedDeltaTime);
                }
            }
            commonFixedUpdates.RemoveAll((u) => !IsValid(u));
        }

#endif
#if !GIB_NO_COMMLATEUPDATE

        private void LateUpdate()
        {
            List<ICommonLateUpdate> unmutable = new List<ICommonLateUpdate>(commonLateUpdates);
            foreach (ICommonLateUpdate update in unmutable)
            {
                if (CanUpdate(update))
                {
                    update.CommonLateUpdate(Time.deltaTime);
                }
            }
            commonLateUpdates.RemoveAll((u) => !IsValid(u));
        }

#endif

#if !GIB_NO_COMMUPDATE

        private void Update()
        {
            List<ICommonUpdate> unmutable = new List<ICommonUpdate>(commonUpdates);
            foreach (ICommonUpdate update in unmutable)
            {
                if (CanUpdate(update))
                {
                    update.CommonUpdate(Time.deltaTime);
                }
            }
            commonUpdates.RemoveAll((u) => !IsValid(u));
        }

#endif
    }
}
