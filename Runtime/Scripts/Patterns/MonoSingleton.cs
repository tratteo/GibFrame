using UnityEngine;

namespace GibFrame.Patterns
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        [Tooltip("Whether the gameObject should persist throughout scenes")]
        [Header("Singleton")]
        [SerializeField] private bool persistent = false;

        public static T Instance { get; private set; }

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
        }
    }
}