//Copyright (c) matteo
//GDebug.cs - com.tratteo.gibframe

using UnityEngine;

namespace GibFrame
{
    public static partial class GDebug
    {
        public static void DrawWireBox(Box box, Color color, float duration)
        {
            UnityEngine.Debug.DrawLine(box.FrontTopLeft, box.FrontTopRight, color, duration);
            UnityEngine.Debug.DrawLine(box.FrontTopRight, box.FrontBottomRight, color, duration);
            UnityEngine.Debug.DrawLine(box.FrontBottomRight, box.FrontBottomLeft, color, duration);
            UnityEngine.Debug.DrawLine(box.FrontBottomLeft, box.FrontTopLeft, color, duration);
            UnityEngine.Debug.DrawLine(box.BackTopLeft, box.BackTopRight, color, duration);
            UnityEngine.Debug.DrawLine(box.BackTopRight, box.BackBottomRight, color, duration);
            UnityEngine.Debug.DrawLine(box.BackBottomRight, box.BackBottomLeft, color, duration);
            UnityEngine.Debug.DrawLine(box.BackBottomLeft, box.BackTopLeft, color, duration);
            UnityEngine.Debug.DrawLine(box.FrontTopLeft, box.BackTopLeft, color, duration);
            UnityEngine.Debug.DrawLine(box.FrontTopRight, box.BackTopRight, color, duration);
            UnityEngine.Debug.DrawLine(box.FrontBottomRight, box.BackBottomRight, color, duration);
            UnityEngine.Debug.DrawLine(box.FrontBottomLeft, box.BackBottomLeft, color, duration);
        }

        public static void DrawWireBox(Box box)
        {
            DrawWireBox(box, Color.cyan, 0F);
        }

        public static void DrawWireBox(Box box, Color color)
        {
            DrawWireBox(box, color, 0F);
        }

        public static void DrawWireBox(Box box, float duration)
        {
            DrawWireBox(box, Color.cyan, duration);
        }

        public static void DrawWireSphere(Vector3 center, float radius, Color color, float duration, int quality = 3)
        {
            quality = Mathf.Clamp(quality, 1, 10);

            int segments = quality << 1;
            int subdivisions = quality << 2;
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

        public static void DrawCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, float duration)
        {
            direction.Normalize();
            Box bottomBox = new Box(origin, halfExtents, orientation);
            Box topBox = new Box(origin + (direction * distance), halfExtents, orientation);

            DrawArrow(bottomBox.BackBottomLeft, topBox.BackBottomLeft - bottomBox.BackBottomLeft, Color.yellow, duration);
            DrawArrow(bottomBox.BackBottomRight, topBox.BackBottomRight - bottomBox.BackBottomRight, Color.yellow, duration);
            DrawArrow(bottomBox.BackTopLeft, topBox.BackTopLeft - bottomBox.BackTopLeft, Color.yellow, duration);
            DrawArrow(bottomBox.BackTopRight, topBox.BackTopRight - bottomBox.BackTopRight, Color.yellow, duration);

            DrawArrow(bottomBox.FrontTopLeft, topBox.FrontTopLeft - bottomBox.FrontTopLeft, Color.yellow, duration);
            DrawArrow(bottomBox.FrontTopRight, topBox.FrontTopRight - bottomBox.FrontTopRight, Color.yellow, duration);
            DrawArrow(bottomBox.FrontBottomLeft, topBox.FrontBottomLeft - bottomBox.FrontBottomLeft, Color.yellow, duration);
            DrawArrow(bottomBox.FrontBottomRight, topBox.FrontBottomRight - bottomBox.FrontBottomRight, Color.yellow, duration);

            DrawWireBox(bottomBox, Color.green, duration);
            DrawWireBox(topBox, Color.green, duration);
        }
    }
}