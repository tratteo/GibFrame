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
    /// <summary>
    ///   The runtime allowing for common updates. Boost performance when managing scenes with a lot of entities
    /// </summary>
    public class CommonUpdateRuntime : MonoBehaviour
    {
        [Header("Preferences")]
        [SerializeField] protected bool persistent = false;
        [SerializeField] protected HideFlags flags;
        private readonly List<ICommonFixedUpdate> commonFixedUpdates = new List<ICommonFixedUpdate>();

        private readonly List<ICommonLateUpdate> commonLateUpdates = new List<ICommonLateUpdate>();

        private readonly List<ICommonUpdate> commonUpdates = new List<ICommonUpdate>();
        [Tooltip("Scan for all MonoBehaviours on scene loading")]
        [SerializeField] private bool scanOnSceneLoading = false;

        [Tooltip("Whether should disabled gameobjects be updated")]
        [SerializeField] private bool updateDisabled = false;

        private static CommonUpdateRuntime Instance { get; set; }

        /// <summary>
        ///   Install the class in the common update runtime
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="update"> </param>
        /// <returns>
        ///   The number of successful installs for this object. <strong> -1 </strong> if the instance of the <see
        ///   cref="CommonUpdateRuntime"/> is <strong> <c> null </c></strong>
        /// </returns>
        public static int Install<T>(T update) where T : class
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<CommonUpdateRuntime>();
#if !GIB_NO_COMM_RUNTIME_INSTANTIATE
                if (Instance == null)
                {
                    var obj = new GameObject()
                    {
                        name = "CommonUpdateManagerRuntime"
                    };
                    Instance = obj.AddComponent<CommonUpdateRuntime>();
                }
#endif
            }
            if (Instance != null)
            {
                var registrations = 0;
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
                Debug.LogWarning("Common updating is being used but the Instance of the CommonUpdateManager is null. Either add a CommonUpdateManager in the scene or enable auto runtime instantiation in GibFrame settings");
                return -1;
            }
        }

        /// <summary>
        ///   Uninstall the class in the common update runtime
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="update"> </param>
        /// <returns>
        ///   The number of successful uninstalls for this object. <strong> -1 </strong> if the instance of the <see
        ///   cref="CommonUpdateRuntime"/> is <strong> <c> null </c></strong>
        /// </returns>
        public static int Uninstall<T>(T update) where T : class
        {
            if (Instance == null)
            {
                Instance = FindObjectOfType<CommonUpdateRuntime>();
            }
            if (Instance != null)
            {
                var unregistrations = 0;
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

        /// <summary>
        ///   Perform a clean reinstallation
        /// </summary>
        public void CleanAndRescan()
        {
            commonUpdates.Clear();
            commonFixedUpdates.Clear();
            commonLateUpdates.Clear();
            Scan();
        }

        /// <summary>
        ///   Scan the scene and install new behaviours
        /// </summary>
        public void Scan()
        {
            var monos = FindObjectsOfType<MonoBehaviour>();
            foreach (var mono in monos)
            {
                Install(mono);
            }
        }

        protected void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }

            if (persistent)
            {
                DontDestroyOnLoad(gameObject);
            }
            gameObject.hideFlags = flags;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private bool CanUpdate<T>(T update) => update is MonoBehaviour mono ? mono && (mono.enabled && (updateDisabled || mono.gameObject.activeSelf)) : update != null;

        private bool IsValid<T>(T update) => update is MonoBehaviour mono ? (bool)mono : update != null;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scanOnSceneLoading)
            {
                CleanAndRescan();
            }
        }

#if !GIB_NO_COMMFIXEDUPDATE

        private void FixedUpdate()
        {
            var unmutable = new List<ICommonFixedUpdate>(commonFixedUpdates);
            foreach (var update in unmutable)
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
            var unmutable = new List<ICommonLateUpdate>(commonLateUpdates);
            foreach (var update in unmutable)
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
            var unmutable = new List<ICommonUpdate>(commonUpdates);
            foreach (var update in unmutable)
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