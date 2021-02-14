// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.Components : SmoothFollow.cs
//
// All Rights Reserved

using UnityEngine;

namespace GibFrame.Components
{
    /// <summary>
    ///   Attach this component to a camera and select a target in the editor, the camera will follow it. Can adjust the offset and the hardness
    /// </summary>
    public class SmoothFollow : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        public float hardness = 5f;

        private void FixedUpdate()
        {
            if (target != null)
            {
                Vector3 expectedPosition = target.position + offset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, expectedPosition, hardness * Time.deltaTime);
                transform.position = smoothedPosition;
            }
        }
    }
}