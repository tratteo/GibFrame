// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : RotationRelator.cs
//
// All Rights Reserved

using UnityEngine;

namespace GibFrame
{
    public class RotationRelator : MonoBehaviour
    {
        public bool disjoinX;
        public bool disjoinY;
        public bool disjoinZ;
        private Vector3 disjointRotation;

        public void Disjoin(bool x, bool y, bool z)
        {
            disjoinX = x;
            disjoinY = y;
            disjoinZ = z;
        }

        public void RotateAroundDisjoint(Vector3 axis, float angle)
        {
            disjointRotation += axis * angle;
        }

        private void Awake()
        {
            disjointRotation = transform.rotation.eulerAngles;
        }

        private void LateUpdate()
        {
            if (IsDisjointed())
            {
                var xRot = disjoinX ? disjointRotation.x : transform.rotation.eulerAngles.x;
                var yRot = disjoinY ? disjointRotation.y : transform.rotation.eulerAngles.y;
                var zRot = disjoinZ ? disjointRotation.z : transform.rotation.eulerAngles.z;
                transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
            }
        }

        private bool IsDisjointed()
        {
            return disjoinX || disjoinY || disjoinZ;
        }
    }
}
