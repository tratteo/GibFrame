using UnityEngine;

namespace GibFrame.Debug
{
    public static class GDebug
    {
        public static void DrawWireCube(Vector3 center, Vector3 halfExtents, Color color, float duration)
        {
            Vector3 a, b, c, d;
            a = new Vector3(center.x - halfExtents.x, center.y - halfExtents.y, center.z - halfExtents.z);
            b = new Vector3(center.x - halfExtents.x, center.y - halfExtents.y, center.z + halfExtents.z);
            c = new Vector3(center.x + halfExtents.x, center.y - halfExtents.y, center.z + halfExtents.z);
            d = new Vector3(center.x + halfExtents.x, center.y - halfExtents.y, center.z - halfExtents.z);
            UnityEngine.Debug.DrawLine(a, b, color, duration);
            UnityEngine.Debug.DrawLine(b, c, color, duration);
            UnityEngine.Debug.DrawLine(c, d, color, duration);
            UnityEngine.Debug.DrawLine(d, a, color, duration);
            Vector3 _a = a + Vector3.up * 2F * halfExtents.y;
            Vector3 _b = b + Vector3.up * 2F * halfExtents.y;
            Vector3 _c = c + Vector3.up * 2F * halfExtents.y;
            Vector3 _d = d + Vector3.up * 2F * halfExtents.y;
            UnityEngine.Debug.DrawLine(_a, _b, color, duration);
            UnityEngine.Debug.DrawLine(_b, _c, color, duration);
            UnityEngine.Debug.DrawLine(_c, _d, color, duration);
            UnityEngine.Debug.DrawLine(_d, _a, color, duration);

            UnityEngine.Debug.DrawLine(_a, a, color, duration);
            UnityEngine.Debug.DrawLine(_b, b, color, duration);
            UnityEngine.Debug.DrawLine(_c, c, color, duration);
            UnityEngine.Debug.DrawLine(_d, d, color, duration);
        }

        public static void DrawWireCube(Vector3 center, Vector3 halfExtents)
        {
            DrawWireCube(center, halfExtents, Color.cyan, 0F);
        }

        public static void DrawWireCube(Vector3 center, Vector3 halfExtents, Color color)
        {
            DrawWireCube(center, halfExtents, color, 0F);
        }

        public static void DrawWireCube(Vector3 center, Vector3 halfExtents, float duration)
        {
            DrawWireCube(center, halfExtents, Color.cyan, duration);
        }

        /// <summary>
        /// </summary>
        /// <param name="center"> </param>
        /// <param name="radius"> </param>
        /// <param name="color"> </param>
        /// <param name="duration"> </param>
        /// <param name="quality"> Define the quality of the wire sphere, from 1 to 10 </param>
        public static void DrawWireSphere(Vector3 center, float radius, Color color, float duration, int quality = 3)
        {
            quality = Mathf.Clamp(quality, 1, 10);

            int segments = quality << 1;
            int subdivisions = quality << 3;
            int halfSegments = segments >> 1;
            float strideAngle = 360F / subdivisions;
            float segmentStride = 180F / segments;

            Vector3 first;
            Vector3 next;
            for (int i = 0; i < segments; i++)
            {
                first = (Vector3.forward * radius);
                first = Quaternion.AngleAxis(segmentStride * (i - halfSegments), Vector3.right) * first;

                for (int j = 0; j < subdivisions; j++)
                {
                    next = Quaternion.AngleAxis(strideAngle, Vector3.up) * first;
                    UnityEngine.Debug.DrawLine(first + center, next + center, color, duration);
                    first = next;
                }
            }

            Vector3 axis;
            for (int i = 0; i < segments; i++)
            {
                first = (Vector3.forward * radius);
                first = Quaternion.AngleAxis(segmentStride * (i - halfSegments), Vector3.up) * first;
                axis = Quaternion.AngleAxis(90F, Vector3.up) * first;

                for (int j = 0; j < subdivisions; j++)
                {
                    next = Quaternion.AngleAxis(strideAngle, axis) * first;
                    UnityEngine.Debug.DrawLine(first + center, next + center, color, duration);
                    first = next;
                }
            }
        }

        public static void DrawWireSphere(Vector3 center, float radius)
        {
            DrawWireSphere(center, radius, Color.cyan, 0F);
        }

        public static void DrawWireSphere(Vector3 center, float radius, Color color)
        {
            DrawWireSphere(center, radius, color, 0F);
        }

        public static void DrawWireSphere(Vector3 center, float radius, float duration)
        {
            DrawWireSphere(center, radius, Color.cyan, duration);
        }

        public static void DrawArrow(Vector3 pos, Vector3 direction, Color color, float duration, float arrowHeadLength, float arrowHeadAngle)
        {
            UnityEngine.Debug.DrawRay(pos, direction, color, duration);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            UnityEngine.Debug.DrawRay(pos + direction, right * arrowHeadLength, color, duration);
            UnityEngine.Debug.DrawRay(pos + direction, left * arrowHeadLength, color, duration);
        }

        public static void DrawArrow(Vector3 pos, Vector3 direction, Color color, float duration)
        {
            DrawArrow(pos, direction, color, duration, 0.25F, 25F);
        }

        public static void DrawArrow(Vector3 pos, Vector3 direction, Color color)
        {
            DrawArrow(pos, direction, color, 0F, 0.25F, 25F);
        }

        public static void DrawArrow(Vector3 pos, Vector3 direction, float duration)
        {
            DrawArrow(pos, direction, Color.cyan, duration, 0.25F, 25F);
        }

        public static void DrawArrow(Vector3 pos, Vector3 direction)
        {
            DrawArrow(pos, direction, Color.cyan, 0F, 0.25F, 25F);
        }
    }
}
