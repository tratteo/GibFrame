// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.Utils : UIUtils.cs
//
// All Rights Reserved

using System;
using System.Collections;
using System.Text;
using GibFrame.Utils.Callbacks;
using GibFrame.Utils.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace GibFrame.Utils
{
    public class UIUtils
    {
        /// <summary>
        ///   Coroutine that fills an image in a specified time. (Image has to be set to filled image)
        /// </summary>
        /// <param name="context"> </param>
        /// <param name="image"> </param>
        /// <param name="duration"> </param>
        /// <returns> IEnumerator reference </returns>
        public static IEnumerator FillImage(MonoBehaviour context, Image image, float duration, bool realtime = false, float targetFillAmount = 1F, AbstractCallback callback = null)
        {
            IEnumerator coroutine = FillImage_C(image, duration, targetFillAmount, realtime, callback);
            context.StartCoroutine(coroutine);
            return coroutine;
        }

        /// <summary>
        ///   Draw a line from a point to another
        /// </summary>
        public static void DrawSpriteLine(Vector3 from, Vector3 to, Image lineImage, float thickness = 1F, Transform parent = null)
        {
            DrawSpriteLine_A(from, to, thickness, lineImage, parent);
        }

        /// <summary>
        ///   Use {0} to refer the hours, {1} minutes and {2} seconds in the format string
        /// </summary>
        /// <param name="seconds"> </param>
        /// <returns> The formatted string </returns>
        public static string GetTimeStringFromSeconds(float seconds, string format = "{0} h : {1} m : {2} s ")
        {
            int hours = 0;
            int minutes = 0;
            while (seconds / 3600 >= 1)
            {
                hours++;
                seconds -= 3600;
            }
            while (seconds / 60 >= 1)
            {
                minutes++;
                seconds -= 60;
            }
            return string.Format(format, hours, minutes, seconds);
        }

        private static IEnumerator FillImage_C(Image image, float duration, float targetFillAmount, bool realtime, AbstractCallback Callback)
        {
            float diff = GMath.Abs(image.fillAmount - targetFillAmount);
            float stride = Time.fixedDeltaTime / duration;
            stride *= diff;
            if (targetFillAmount - image.fillAmount > 0F)
            {
                while (image.fillAmount + stride < targetFillAmount)
                {
                    if (realtime)
                    {
                        yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                    }
                    else
                    {
                        yield return new WaitForFixedUpdate();
                    }
                    image.fillAmount += stride;
                }
            }
            else
            {
                while (image.fillAmount - stride > targetFillAmount)
                {
                    if (realtime)
                    {
                        yield return new WaitForSecondsRealtime(Time.fixedDeltaTime);
                    }
                    else
                    {
                        yield return new WaitForFixedUpdate();
                    }
                    image.fillAmount -= stride;
                }
            }

            image.fillAmount = targetFillAmount;
            Callback?.Invoke();
        }

        private static void DrawSpriteLine_A(Vector3 from, Vector3 to, float thickness, Image lineImage, Transform parent)
        {
            Image linkRef = UnityEngine.Object.Instantiate(lineImage);
            if (parent != null)
            {
                linkRef.transform.SetParent(parent);
            }

            Vector3 differenceVector = to - from;
            linkRef.rectTransform.sizeDelta = new Vector2(differenceVector.magnitude, thickness);
            linkRef.rectTransform.pivot = new Vector2(0, 0.5f);
            linkRef.rectTransform.position = from;
            float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
            linkRef.rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
        }

        public static class DrawArrow
        {
            public static void ForGizmo(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
            {
                Gizmos.DrawRay(pos, direction);

                Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
                Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
            }

            public static void ForGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
            {
                Gizmos.color = color;
                Gizmos.DrawRay(pos, direction);

                Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
                Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
            }

            public static void ForDebug(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
            {
                Debug.DrawRay(pos, direction);

                Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Debug.DrawRay(pos + direction, right * arrowHeadLength);
                Debug.DrawRay(pos + direction, left * arrowHeadLength);
            }

            public static void ForDebug(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
            {
                Debug.DrawRay(pos, direction, color);

                Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) * new Vector3(0, 0, 1);
                Debug.DrawRay(pos + direction, right * arrowHeadLength, color);
                Debug.DrawRay(pos + direction, left * arrowHeadLength, color);
            }
        }
    }
}
