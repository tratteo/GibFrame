//Copyright (c) matteo
//CameraEffects.cs - com.tratteo.gibframe

using System;
using System.Collections;
using UnityEngine;

namespace GibFrame.CameraUtils
{
    public class CameraEffects
    {
        public bool IsEffectRunning { get; private set; } = false;

        public CameraEffects()
        {
        }

        public void ZoomEffect(MonoBehaviour context, Camera camera, Vector3 target, Vector3 offsetFromTarget, EffectParameters parameters, Action OnZoomedIn = null, Action OnWaitFinished = null, Action OnZoomedOut = null)
        {
            if (IsEffectRunning) return;
            context.StartCoroutine(ZoomEffect_C(camera, target, offsetFromTarget, parameters, OnZoomedIn, OnWaitFinished, OnZoomedOut));
        }

        private IEnumerator ZoomEffect_C(Camera camera, Vector3 target, Vector3 offsetFromTarget, EffectParameters parameters, Action OnZoomedIn = null, Action OnWaitFinished = null, Action OnZoomedOut = null)
        {
            WaitForSecondsRealtime wait = new WaitForSecondsRealtime(Time.fixedDeltaTime);
            Quaternion initialCameraRot = camera.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(target - camera.transform.position);

            IsEffectRunning = true;
            Vector3 initialPos = camera.transform.position;
            Vector3 effectiveTarget = target + offsetFromTarget;
            float distance = Vector3.Distance(effectiveTarget, camera.transform.position);
            Vector3 moveDir = effectiveTarget - camera.transform.position;
            moveDir.Normalize();

            float coeff = (float)(distance / Math.Pow(parameters.Timings.inTransitionTime, parameters.MathematicalTrend.strength));
            float current = Time.fixedDeltaTime;
            float times = parameters.Timings.inTransitionTime / Time.fixedDeltaTime;
            while (current < parameters.Timings.inTransitionTime || (parameters.LookAt && Quaternion.Angle(camera.transform.rotation, targetRotation) >= 1))
            {
                if (current < parameters.Timings.inTransitionTime)
                {
                    camera.transform.position = initialPos + moveDir * (parameters.MathematicalTrend.Calculate(current, parameters.Timings.inTransitionTime, distance, coeff));
                }
                else
                {
                    camera.transform.position = effectiveTarget;
                }
                current += Time.fixedDeltaTime;
                if (parameters.LookAt)
                {
                    camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, targetRotation, parameters.InterpolationStrenght * Time.fixedDeltaTime);
                }
                yield return wait;
            }

            camera.transform.position = effectiveTarget;
            OnZoomedIn?.Invoke();
            yield return new WaitForSecondsRealtime(parameters.Timings.time);
            OnWaitFinished?.Invoke();
            coeff = (float)(distance / Math.Pow(parameters.Timings.outTransitionTime, parameters.MathematicalTrend.strength));
            current = Time.fixedDeltaTime;

            while (current < parameters.Timings.outTransitionTime || (parameters.LookAt && Quaternion.Angle(camera.transform.rotation, initialCameraRot) >= 1))
            {
                if (current < parameters.Timings.outTransitionTime)
                {
                    camera.transform.position = effectiveTarget - moveDir * (parameters.MathematicalTrend.Calculate(current, parameters.Timings.outTransitionTime, distance, coeff));
                }
                else
                {
                    camera.transform.position = initialPos;
                }
                current += Time.fixedDeltaTime;
                if (parameters.LookAt)
                {
                    camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, initialCameraRot, parameters.InterpolationStrenght * Time.fixedDeltaTime);
                }
                yield return wait;
            }

            OnZoomedOut?.Invoke();
            IsEffectRunning = false;
        }

        public class EffectParameters
        {
            internal Trend MathematicalTrend { get; private set; }

            internal bool LookAt { get; private set; }

            internal Times Timings { get; private set; }

            internal float InterpolationStrenght { get; private set; }

            private EffectParameters()
            {
                Timings = new Times() { time = 1, inTransitionTime = 1, outTransitionTime = 1 };
                MathematicalTrend = new Trend() { strength = 1F, mathematicalTrend = Trend.Mathematical.LINEAR };
                LookAt = true;
            }

            public static Builder Create() => new Builder();

            public struct Times
            {
                internal float time;
                internal float inTransitionTime;
                internal float outTransitionTime;

                internal Times(float time, float inTransitionTime, float outTransitionTime)
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

                internal Trend(float strength, Mathematical mathematicalTrend)
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

            public class Builder
            {
                private EffectParameters effect;

                public Builder()
                {
                    effect = new EffectParameters();
                }

                public static implicit operator EffectParameters(Builder builder) => builder.effect;

                public Builder WithTimings(float time = 1, float inTransitionTime = 1, float outTransitionTime = 1)
                {
                    effect.Timings = new Times(time, inTransitionTime, outTransitionTime);
                    return this;
                }

                public Builder WithTrend(float strength = 1, Trend.Mathematical mathematicalTrend = Trend.Mathematical.LINEAR)
                {
                    effect.MathematicalTrend = new Trend(strength, mathematicalTrend);
                    return this;
                }

                public Builder LookAtTarget(float strenght)
                {
                    effect.LookAt = true;
                    effect.InterpolationStrenght = strenght;
                    return this;
                }
            }
        }
    }
}