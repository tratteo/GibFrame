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

        public static void DrawArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            UnityEngine.Debug.DrawRay(pos, direction);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            UnityEngine.Debug.DrawRay(pos + direction, right * arrowHeadLength);
            UnityEngine.Debug.DrawRay(pos + direction, left * arrowHeadLength);
        }

        public static void DrawArrow(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
        {
            UnityEngine.Debug.DrawRay(pos, direction, color);

            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
            UnityEngine.Debug.DrawRay(pos + direction, right * arrowHeadLength, color);
            UnityEngine.Debug.DrawRay(pos + direction, left * arrowHeadLength, color);
        }
    }
}
