using System.Collections.Generic;
using GibFrame.Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GibFrame.Performance
{
    public class CommonUpdateManager : MonoSingleton<CommonUpdateManager>
    {
        [Header("Preferences")]
        [SerializeField] private bool autoScanOnStartup = false;
        private List<ICommonFixedUpdate> commonFixedUpdates = new List<ICommonFixedUpdate>();
        private List<ICommonLateUpdate> commonLateUpdates = new List<ICommonLateUpdate>();
        private List<ICommonUpdate> commonUpdates = new List<ICommonUpdate>();

        public void Register(MonoBehaviour mono)
        {
            ICommonUpdate update;
            ICommonFixedUpdate fixedUpdate;
            ICommonLateUpdate lateUpdate;
            if ((update = mono.GetComponent<ICommonUpdate>()) != null)
            {
                Register(update);
            }
            if ((fixedUpdate = mono.GetComponent<ICommonFixedUpdate>()) != null)
            {
                Register(fixedUpdate);
            }
            if ((lateUpdate = mono.GetComponent<ICommonLateUpdate>()) != null)
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
            commonUpdates.Clear();
            commonFixedUpdates.Clear();
            commonLateUpdates.Clear();
            MonoBehaviour[] monos = FindObjectsOfType<MonoBehaviour>();
            int lenght = monos.Length;
            for (int i = 0; i < lenght; i++)
            {
                Register(monos[i]);
            }
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

        private void FixedUpdate()
        {
            foreach (ICommonFixedUpdate update in commonFixedUpdates)
            {
                MonoBehaviour mono = update as MonoBehaviour;
                if (mono == null || (mono != null && mono.enabled && mono.gameObject.activeSelf))
                {
                    update.CommonFixedUpdate(Time.fixedDeltaTime);
                }
            }
            commonFixedUpdates.RemoveAll((u) => u == null);
        }

        private void LateUpdate()
        {
            foreach (ICommonLateUpdate update in commonLateUpdates)
            {
                MonoBehaviour mono = update as MonoBehaviour;
                if (mono == null || (mono != null && mono.enabled && mono.gameObject.activeSelf))
                {
                    update.CommonLateUpdate();
                }
            }
            commonLateUpdates.RemoveAll((u) => u == null);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (autoScanOnStartup)
            {
                Rescan();
            }
        }

#if !GIB_NO_COMMUPDATE

        private void Update()
        {
            foreach (ICommonUpdate update in commonUpdates)
            {
                MonoBehaviour mono = update as MonoBehaviour;
                if (mono == null || (mono != null && mono.enabled && mono.gameObject.activeSelf))
                {
                    update.CommonUpdate(Time.deltaTime);
                }
            }

            commonUpdates.RemoveAll((u) => u == null);
        }

#endif

#if !GIB_NO_COMMLATEUPDATE
#endif

#if !GIB_NO_COMMFIXEDUPDATE
    }

#endif
}
