// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : ParticleSystemAutoDestroy.cs
//
// All Rights Reserved

using UnityEngine;

namespace GibFrame
{
    /// <summary>
    ///   Attach this component to a particleSystem, once the particle system is stopped od finished the script will destory the gameObject
    /// </summary>
    public class ParticleSystemAutoInhibit : MonoBehaviour
    {
        public enum Action { DISABLE, DESTROY }

        private new ParticleSystem particleSystem;

        [SerializeField]
        private Action onSimulationEnded;

        public void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        public void Update()
        {
            if (particleSystem != null)
            {
                if (!particleSystem.IsAlive())
                {
                    switch (onSimulationEnded)
                    {
                        case Action.DESTROY:
                            Destroy(gameObject);
                            break;

                        case Action.DISABLE:
                            gameObject.SetActive(false);
                            break;
                    }
                }
            }
        }
    }
}
