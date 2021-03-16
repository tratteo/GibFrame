using System.Collections.Generic;
using GibFrame.Patterns;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GibFrame.Performance
{
    public class CommonUpdateManager : MonoSingleton<CommonUpdateManager>
    {
        private List<ICommonUpdate> commonUpdates = new List<ICommonUpdate>();
        private List<ICommonFixedUpdate> commonFixedUpdates = new List<ICommonFixedUpdate>();
        private List<ICommonLateUpdate> commonLateUpdates = new List<ICommonLateUpdate>();

        public void Rescan()
        {
            commonUpdates.Clear();
            commonFixedUpdates.Clear();
            commonLateUpdates.Clear();
            MonoBehaviour[] monos = FindObjectsOfType<MonoBehaviour>();
            int lenght = monos.Length;
            for (int i = 0; i < lenght; i++)
            {
                ICommonUpdate update;
                ICommonFixedUpdate fixedUpdate;
                ICommonLateUpdate lateUpdate;
                if ((update = monos[i].GetComponent<ICommonUpdate>()) != null)
                {
                    commonUpdates.Add(update);
                }
                if ((fixedUpdate = monos[i].GetComponent<ICommonFixedUpdate>()) != null)
                {
                    commonFixedUpdates.Add(fixedUpdate);
                }
                if ((lateUpdate = monos[i].GetComponent<ICommonLateUpdate>()) != null)
                {
                    commonLateUpdates.Add(lateUpdate);
                }
            }
        }

        public void NotifyCommonUpdate(ICommonUpdate update)
        {
            commonUpdates.Add(update);
        }

        public void NotifyCommonLateUpdate(ICommonLateUpdate update)
        {
            commonLateUpdates.Add(update);
        }

        public void NotifyCommonFixedUpdate(ICommonFixedUpdate update)
        {
            commonFixedUpdates.Add(update);
        }

        protected override void Awake()
        {
            base.Awake();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Rescan();
        }

#if !INHIBIT_UPDATE

        private void Update()
        {
            foreach (ICommonUpdate update in commonUpdates)
            {
                if (update != null)
                {
                    update.CommonUpdate();
                }
            }

            commonUpdates.RemoveAll((u) => u == null);
        }

#endif

#if !INHIBIT_LATE_UPDATE

        private void LateUpdate()
        {
            foreach (ICommonLateUpdate update in commonLateUpdates)
            {
                if (update != null)
                {
                    update.CommonLateUpdate();
                }
            }
            commonLateUpdates.RemoveAll((u) => u == null);
        }

#endif

#if !INHIBIT_FIXED_UPDATE

        private void FixedUpdate()
        {
            foreach (ICommonFixedUpdate update in commonFixedUpdates)
            {
                if (update != null)
                {
                    update.CommonFixedUpdate();
                }
            }
            commonFixedUpdates.RemoveAll((u) => u == null);
        }
    }

#endif
}
