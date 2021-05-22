//Copyright (c) matteo
//Follow.cs - com.tratteo.gibframe

using UnityEngine;

namespace GibFrame
{
    /// <summary>
    ///   Attach this component to a gameObject and specify a target transform, the gameObject with this script attached will follow the
    ///   target transform
    /// </summary>
    public class Follow : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;

        public bool Active { get; private set; } = true;

        public void SetActive(bool active)
        {
            Active = active;
        }

        private void Start()
        {
            transform.position = target.position + offset;
        }

        private void Update()
        {
            if (Active)
            {
                transform.position = target.position + offset;
            }
        }
    }
}