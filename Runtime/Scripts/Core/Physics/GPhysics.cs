// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : GPhysics.cs
//
// All Rights Reserved

using System;
using UnityEngine;

namespace GibFrame
{
    public static class GPhysics
    {
        public static Vector3 CalculateThrowVelocity(Vector3 origin, Vector3 target, float angle, float gravityMagnitude)
        {
            angle *= Mathf.Deg2Rad;
            float tan = Mathf.Tan(angle);
            float Dx = Vector3.Distance(origin, new Vector3(target.x, origin.y, target.z));
            float Dy = GMath.Abs(target.y - origin.y);
            float vi = (Dx / Mathf.Cos(angle)) * (Mathf.Sqrt(gravityMagnitude) / Mathf.Sqrt(2F * (Dx * tan + Dy)));
            Vector3 planeDir = (new Vector3(target.x, origin.y, target.z) - origin);
            return new Vector3(planeDir.x, planeDir.magnitude * tan, planeDir.z).normalized * vi * (1F + Time.fixedDeltaTime);
        }

        public static Vector3 CalculateThrowVelocity(Vector3 origin, Vector3 target, float angle)
        {
            return CalculateThrowVelocity(origin, target, angle, Physics.gravity.magnitude);
        }
    }
}
