//Copyright (c) matteo
//CameraManager.cs - com.tratteo.gibframe

using System;
using System.Collections;
using GibFrame.Patterns;
using UnityEngine;

namespace GibFrame.CameraUtils
{
    /// <summary>
    ///   Attach this component to a GameObject and insert all the cameras in the slot, then use this component to acces and manipulate them
    /// </summary>
    public class CameraManager : MonoSingleton<CameraManager>
    {
        [SerializeField] private CameraInfo[] cameras;

        public CameraEffects Effects { get; private set; }

        /// <summary>
        ///   Smooth in and then out a camera FOV to a target FOV in transitionDuration time, for duration time. If cameraID is null, all
        ///   the cameras will be affected
        /// </summary>
        /// <param name="cameraID"> </param>
        /// <param name="targetFOV"> </param>
        /// <param name="transitionDuration"> </param>
        /// <param name="duration"> </param>
        public void SmoothInAndOutFOV(string cameraID, float targetFOV, float transitionDuration, float duration)
        {
            if (cameraID == null)
            {
                int length = cameras.Length;
                for (int i = 0; i < length; i++)
                {
                    StartCoroutine(SmoothInAndOutFOVCoroutine(targetFOV, transitionDuration, duration, cameras[i]));
                }
            }
            else
            {
                CameraInfo cameraInfo = Array.Find(cameras, cam => cam.id == cameraID);
                StartCoroutine(SmoothInAndOutFOVCoroutine(targetFOV, transitionDuration, duration, cameraInfo));
            }
        }

        /// <summary>
        ///   Smooth a camera FOV to a target FOV in transitionDuration time, for duration time. If cameraID is null, all the cameras will
        ///   be affected
        /// </summary>
        /// <param name="cameraID"> </param>
        /// <param name="targetFOV"> </param>
        /// <param name="transitionDuration"> </param>
        /// <param name="delay"> </param>
        public void SmoothFOV(string cameraID, float targetFOV, float transitionDuration, float delay)
        {
            if (cameraID == null)
            {
                int length = cameras.Length;
                for (int i = 0; i < length; i++)
                {
                    StartCoroutine(SmoothFOVCoroutine(targetFOV, transitionDuration, delay, cameras[i]));
                }
            }
            else
            {
                CameraInfo cameraInfo = Array.Find(cameras, cam => cam.id == cameraID);
                StartCoroutine(SmoothFOVCoroutine(targetFOV, transitionDuration, delay, cameraInfo));
            }
        }

        /// <summary>
        ///   Reset the FOV of a camera, if passed null all cameras will be affected
        /// </summary>
        /// <param name="cameraID"> </param>
        public void ResetFOV(string cameraID)
        {
            if (cameraID == null)
            {
                int length = cameras.Length;
                for (int i = 0; i < length; i++)
                {
                    cameras[i].camera.fieldOfView = cameras[i].initFOV;
                }
            }
            else
            {
                CameraInfo cameraInfo = Array.Find(cameras, cam => cam.id == cameraID);
                cameraInfo.camera.fieldOfView = cameraInfo.initFOV;
            }
        }

        /// <summary>
        ///   Get the specified camera
        /// </summary>
        /// <param name="cameraID"> </param>
        /// <returns> The camera instance </returns>
        public Camera GetCamera(string cameraID)
        {
            Camera camera = Array.Find(cameras, c => c.id == cameraID).camera;
            return camera;
        }

        protected override void Awake()
        {
            base.Awake();
            int length = cameras.Length;
            for (int i = 0; i < length; i++)
            {
                cameras[i].initFOV = cameras[i].camera.fieldOfView;
            }
            Effects = new CameraEffects();
        }

        private IEnumerator SmoothInAndOutFOVCoroutine(float targetFOV, float transitionDuration, float duration, CameraInfo cameraInfo)
        {
            float initFOV;
            float currentFOV = initFOV = cameraInfo.camera.fieldOfView;

            float stride = Time.fixedDeltaTime / transitionDuration;
            stride *= Mathf.Abs(currentFOV - targetFOV);
            if (targetFOV > currentFOV)
            {
                while (currentFOV + stride <= targetFOV)
                {
                    currentFOV += stride;
                    cameraInfo.camera.fieldOfView = currentFOV;
                    yield return new WaitForFixedUpdate();
                }
                cameraInfo.camera.fieldOfView = currentFOV = targetFOV;

                yield return new WaitForSeconds(duration);

                while (currentFOV - stride >= initFOV)
                {
                    currentFOV -= stride;
                    cameraInfo.camera.fieldOfView = currentFOV;
                    yield return new WaitForFixedUpdate();
                }
                cameraInfo.camera.fieldOfView = initFOV;
            }
            else if (targetFOV < currentFOV)
            {
                while (currentFOV - stride >= targetFOV)
                {
                    currentFOV -= stride;
                    cameraInfo.camera.fieldOfView = currentFOV;
                    yield return new WaitForFixedUpdate();
                }
                cameraInfo.camera.fieldOfView = targetFOV;

                yield return new WaitForSeconds(duration);

                while (currentFOV + stride >= initFOV)
                {
                    currentFOV += stride;
                    cameraInfo.camera.fieldOfView = currentFOV;
                    yield return new WaitForFixedUpdate();
                }
                cameraInfo.camera.fieldOfView = initFOV;
            }
        }

        private IEnumerator SmoothFOVCoroutine(float targetFOV, float transitionDuration, float delay, CameraInfo cameraInfo)
        {
            float currentFOV = cameraInfo.camera.fieldOfView;
            float stride = Time.fixedDeltaTime / Mathf.Abs(currentFOV - targetFOV);

            yield return new WaitForSeconds(delay);
            if (targetFOV > currentFOV)
            {
                while (currentFOV + stride <= targetFOV)
                {
                    currentFOV += stride;
                    yield return new WaitForFixedUpdate();
                    cameraInfo.camera.fieldOfView = currentFOV;
                }
                cameraInfo.camera.fieldOfView = targetFOV;
            }
            else if (targetFOV < currentFOV)
            {
                while (currentFOV + stride >= targetFOV)
                {
                    currentFOV -= stride;
                    yield return new WaitForFixedUpdate();
                    cameraInfo.camera.fieldOfView = currentFOV;
                }
                cameraInfo.camera.fieldOfView = targetFOV;
            }
        }

        [System.Serializable]
        public struct CameraInfo
        {
            public string id;
            public Camera camera;
            [HideInInspector] public float initFOV;
        }
    }
}