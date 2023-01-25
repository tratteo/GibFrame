using UnityEngine;

namespace GibFrame
{
    public static class GPhysics
    {
        public static Vector3 CalculateThrowVelocity(Vector3 origin, Vector3 target, float angle, float gravityMagnitude)
        {
            angle *= Mathf.Deg2Rad;
            var tan = Mathf.Tan(angle);
            var Dx = Vector3.Distance(origin, new Vector3(target.x, origin.y, target.z));
            var Dy = Mathf.Abs(target.y - origin.y);
            var vi = (Dx / Mathf.Cos(angle)) * (Mathf.Sqrt(gravityMagnitude) / Mathf.Sqrt(2F * (Dx * tan + Dy)));
            var planeDir = (new Vector3(target.x, origin.y, target.z) - origin);
            return (1F + Time.fixedDeltaTime) * vi * new Vector3(planeDir.x, planeDir.magnitude * tan, planeDir.z).normalized;
        }

        public static Vector3 CalculateThrowVelocity(Vector3 origin, Vector3 target, float angle) => CalculateThrowVelocity(origin, target, angle, Physics.gravity.magnitude);
    }
}