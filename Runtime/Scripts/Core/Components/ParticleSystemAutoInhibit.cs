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
        public enum Action
        { Disable, Destroy }

        public Action onSimulationEnded;
        private new ParticleSystem particleSystem;

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
                        case Action.Destroy:
                            Destroy(gameObject);
                            break;

                        case Action.Disable:
                            gameObject.SetActive(false);
                            break;
                    }
                }
            }
        }
    }
}
