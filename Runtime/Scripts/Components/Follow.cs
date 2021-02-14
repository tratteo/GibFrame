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

        private void Start()
        {
            transform.position = target.position + offset;
        }

        private void Update()
        {
            transform.position = target.position + offset;
        }
    }
}