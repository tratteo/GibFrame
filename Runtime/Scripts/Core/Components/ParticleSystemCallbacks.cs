using UnityEngine;
using UnityEngine.Events;

namespace GibFrame
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemCallbacks : MonoBehaviour
    {
        [SerializeField] private UnityEvent onPlay;
        [SerializeField] private UnityEvent onStop;
        private ParticleSystem system;
        private ParticleSystem.MainModule mainModule;
        private bool cachedState = false;

        private void Awake()
        {
            system = GetComponent<ParticleSystem>();
            mainModule = system.main;
        }

        private void Update()
        {
            if (cachedState != system.isEmitting)
            {
                cachedState = system.isEmitting;
                if (cachedState)
                {
                    onPlay.Invoke();
                }
                else
                {
                    onStop.Invoke();
                }
            }
        }
    }
}
