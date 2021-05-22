//Copyright (c) matteo
//RotationRelator.cs - com.tratteo.gibframe

using UnityEngine;

namespace GibFrame
{
    public class RotationRelator : MonoBehaviour
    {
        [SerializeField] private bool disjoinX;
        [SerializeField] private bool disjoinY;
        [SerializeField] private bool disjoinZ;
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
                float xRot = disjoinX ? disjointRotation.x : transform.rotation.eulerAngles.x;
                float yRot = disjoinY ? disjointRotation.y : transform.rotation.eulerAngles.y;
                float zRot = disjoinZ ? disjointRotation.z : transform.rotation.eulerAngles.z;
                transform.rotation = Quaternion.Euler(xRot, yRot, zRot);
            }
        }

        private bool IsDisjointed()
        {
            return disjoinX || disjoinY || disjoinZ;
        }
    }
}