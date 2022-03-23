// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : SmoothFollow.cs
//
// All Rights Reserved

using UnityEngine;

namespace GibFrame
{
    /// <summary>
    ///   Attach this component to a camera and select a target in the editor, the camera will follow it. Can adjust the offset and the hardness
    /// </summary>
    public class Follower : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        public float hardness = 5f;

        public bool Active { get; private set; } = true;

        public void SetActive(bool active) => Active = active;

        private void FixedUpdate()
        {
            if (target != null && Active)
            {
                var expectedPosition = target.position + offset;
                var smoothedPosition = Vector3.Lerp(transform.position, expectedPosition, hardness * Time.deltaTime);
                transform.position = smoothedPosition;
            }
        }
    }
}
