// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.Components : Follow.cs
//
// All Rights Reserved

using UnityEngine;

namespace GibFrame.Components
{
    /// <summary>
    ///   Attach this component to a gameObject and specify a target transform, the gameObject with this script attached will follow the
    ///   target transform
    /// </summary>
    public class Follow : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;

        public bool Active { get; private set; }

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
