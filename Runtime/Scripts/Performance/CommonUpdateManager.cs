using System.Collections.Generic;
using GibFrame.Extensions;
using GibFrame.Patterns;
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

        public void Register(MonoBehaviour mono)
        {
            if (mono is ICommonUpdate update)
            {
                Register(update);
            }
            if (mono is ICommonFixedUpdate fixedUpdate)
            {
                Register(fixedUpdate);
            }
            if (mono is ICommonLateUpdate lateUpdate)
            {
                Register(lateUpdate);
            }
        }

        public void Register(ICommonUpdate update)
        {
            if (!commonUpdates.Contains(update))
            {
                commonUpdates.Add(update);
            }
        }

        public void Register(ICommonLateUpdate update)
        {
            if (!commonLateUpdates.Contains(update))
            {
                commonLateUpdates.Add(update);
            }
        }

        public void Register(ICommonFixedUpdate update)
        {
            if (!commonFixedUpdates.Contains(update))
            {
                commonFixedUpdates.Add(update);
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

        public void Unregister(MonoBehaviour mono)
        {
            if (mono is ICommonUpdate)
            {
                Unregister(mono as ICommonUpdate);
            }
            if (mono is ICommonLateUpdate)
            {
                Unregister(mono as ICommonLateUpdate);
            }
            if (mono is ICommonFixedUpdate)
            {
                Unregister(mono as ICommonFixedUpdate);
            }
        }

        public bool Unregister(ICommonUpdate update)
        {
            return commonUpdates.Remove(update);
        }

        public bool Unregister(ICommonLateUpdate update)
        {
            return commonLateUpdates.Remove(update);
        }

        public bool Unregister(ICommonFixedUpdate update)
        {
            return commonFixedUpdates.Remove(update);
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
                    update.CommonLateUpdate();
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
