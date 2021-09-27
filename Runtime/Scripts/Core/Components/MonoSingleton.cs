// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : MonoSingleton.cs
//
// All Rights Reserved

using UnityEngine;

namespace GibFrame
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        [Tooltip("Whether the gameObject should persist throughout scenes")]
        [Header("Singleton")]
        [SerializeField] protected bool persistent = false;
        [SerializeField] protected HideFlags flags;

        public static T Instance { get; protected set; }

        [ExecuteAlways]
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else if (Instance != this as T)
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