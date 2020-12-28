using System;
using System.Collections;
using UnityEngine;

namespace GibFrame.CameraUtils
{
    public class CameraEffects
    {
        public bool IsEffectRunning { get; private set; } = false;

        internal CameraEffects()
        {
        }

        public void ZoomEffect(MonoBehaviour context, Camera camera, Vector3 target, Vector3 offsetFromTarget, Times times, Action OnZoomedIn = null, Action OnWaitFinished = null, Action OnZoomedOut = null)
        {
            if (IsEffectRunning) return;
            context.StartCoroutine(ZoomEffect_C(camera, target, offsetFromTarget, times, OnZoomedIn, OnWaitFinished, OnZoomedOut));
        }

        public void ZoomEffect(MonoBehaviour context, Camera camera, Vector3 target, Vector3 offsetFromTarget, Times times, Trend trend, Action OnZoomedIn = null, Action OnWaitFinished = null, Action OnZoomedOut = null)
        {
            if (IsEffectRunning) return;
            context.StartCoroutine(ZoomEffect_C(camera, target, offsetFromTarget, times, trend, OnZoomedIn, OnWaitFinished, OnZoomedOut));
        }

        private IEnumerator ZoomEffect_C(Camera camera, Vector3 target, Vector3 offsetFromTarget, Times times, Trend trend, Action OnZoomedIn = null, Action OnWaitFinished = null, Action OnZoomedOut = null)
        {
            IsEffectRunning = true;
            Vector3 initialPos = camera.transform.position;
            Vector3 effectiveTarget = target + offsetFromTarget;
            float distance = Vector3.Distance(effectiveTarget, camera.transform.position);
            Vector3 moveDir = effectiveTarget - camera.transform.position;
            moveDir.Normalize();

            float coeff = (float)(distance / Math.Pow(times.inTransitionTime, trend.strength));
            //Debug.Log(coeff * trend.Calculate(Time.fixedDeltaTime, times.inTransitionTime, distance, coeff));
            float current = Time.fixedDeltaTime;
            while (current < times.inTransitionTime)
            {
                camera.transform.position = initialPos + moveDir * (trend.Calculate(current, times.inTransitionTime, distance, coeff));
                current += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            camera.transform.position = effectiveTarget;
            OnZoomedIn?.Invoke();
            yield return new WaitForSecondsRealtime(times.time);
            OnWaitFinished?.Invoke();
            coeff = (float)(distance / Math.Pow(times.outTransitionTime, trend.strength));
            current = Time.fixedDeltaTime;
            while (current < times.outTransitionTime)
            {
                camera.transform.position = effectiveTarget - moveDir * (trend.Calculate(current, times.outTransitionTime, distance, coeff));
                current += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            camera.transform.position = initialPos;
            OnZoomedOut?.Invoke();
            IsEffectRunning = false;
        }

        private IEnumerator ZoomEffect_C(Camera camera, Vector3 target, Vector3 offsetFromTarget, Times times, Action OnZoomedIn = null, Action OnWaitFinished = null, Action OnZoomedOut = null)
        {
            IsEffectRunning = true;
            Vector3 initialPos = camera.transform.position;
            Vector3 effectiveTarget = target + offsetFromTarget;
            float distance = Vector3.Distance(effectiveTarget, camera.transform.position);
            Vector3 moveDir = effectiveTarget - camera.transform.position;
            moveDir.Normalize();
            float stride = distance * Time.fixedDeltaTime / times.inTransitionTime;
            float current = 0;
            while (current + stride < distance)
            {
                current += stride;
                camera.transform.position += moveDir * stride;
                yield return new WaitForFixedUpdate();
            }
            current = distance;
            camera.transform.position = effectiveTarget;
            OnZoomedIn?.Invoke();
            yield return new WaitForSecondsRealtime(times.time);
            OnWaitFinished?.Invoke();
            stride = distance * Time.fixedDeltaTime / times.outTransitionTime;
            while (current - stride > 0)
            {
                current -= stride;
                camera.transform.position -= moveDir * stride;
                yield return new WaitForFixedUpdate();
            }
            camera.transform.position = initialPos;
            OnZoomedOut?.Invoke();
            IsEffectRunning = false;
        }

        public struct Times
        {
            internal float time;
            internal float inTransitionTime;
            internal float outTransitionTime;

            public Times(float time, float inTransitionTime, float outTransitionTime)
            {
                this.time = time;
                this.inTransitionTime = inTransitionTime;
                this.outTransitionTime = outTransitionTime;
            }
        }

        public struct Trend
        {
            public enum Mathematical { POLINOMIAL, LINEAR, INVERSE_POL }

            internal float strength;
            internal Mathematical mathematicalTrend;

            public Trend(float strength, Mathematical mathematicalTrend)
            {
                this.strength = strength;
                this.mathematicalTrend = mathematicalTrend;
            }

            internal float Calculate(float value, float time, float distance, float coeff)
            {
                switch (mathematicalTrend)
                {
                    case Mathematical.LINEAR:
                        return value * strength;

                    case Mathematical.POLINOMIAL:
                        return (float)(coeff * Math.Pow(value, strength));

                    case Mathematical.INVERSE_POL:
                        return (float)((-coeff * Math.Pow(value - time, strength)) + distance);
                }
                return default;
            }
        }
    }
}