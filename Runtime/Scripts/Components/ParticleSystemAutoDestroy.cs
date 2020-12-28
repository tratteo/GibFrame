// Copyright (c) 2020 Matteo Beltrame

using UnityEngine;

namespace GibFrame.Components
{
    /// <summary>
    ///   Attach this component to a particleSystem, once the particle system is stopped od finished the script will destory the gameObject
    /// </summary>
    public class ParticleSystemAutoDestroy : MonoBehaviour
    {
        private new ParticleSystem particleSystem;

        public void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        public void Update()
        {
            if (particleSystem)
            {
                if (!particleSystem.IsAlive())
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}