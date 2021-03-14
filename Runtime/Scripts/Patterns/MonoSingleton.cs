//Copyright (c) matteo
//MonoSingleton.cs - com.tratteo.gibframe

using UnityEngine;

namespace GibFrame.Patterns
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        [Tooltip("Whether the gameObject should persist throughout scenes")]
        [Header("Singleton")]
        [SerializeField] protected bool persistent = false;
        [SerializeField] protected HideFlags flags;

        public static T Instance { get; private set; }

        [ExecuteAlways]
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }

            if (persistent)
            {
                DontDestroyOnLoad(gameObject);
            }
            gameObject.hideFlags = flags;
        }
    }
}