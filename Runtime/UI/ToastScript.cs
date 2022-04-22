// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.UI : ToastScript.cs
//
// All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GibFrame.UI
{
    /// <summary>
    ///   Script that manages the behaviour or a toast. In the APIs folder there is a toast prefab. Add it to the scene, use it through a
    ///   reference of its script
    /// </summary>
    public class ToastScript : MonoBehaviour
    {
        private Text text;
        private Image image;
        private bool showing = false;
        private bool coroutineRunning = false;

        private float initImageAlpha;
        private float initTextAlpha;

        private Queue<ToastStruct> toastsQueue = null;

        /// <summary>
        ///   Enqueue a toast message, will be shown when the toast is free from other messages
        /// </summary>
        public void EnqueueToast(string message, Sprite sprite, float duration)
        {
            EnqueueToast(message, sprite, duration, new Vector2(0.5F, 0.5F));
        }

        public void EnqueueToast(string message, Sprite sprite, float duration, Vector2 fadeInOutTimes)
        {
            if (toastsQueue == null)
            {
                toastsQueue = new Queue<ToastStruct>();
            }
            ToastStruct newToast = new ToastStruct(message, sprite, duration, fadeInOutTimes);
            toastsQueue.Enqueue(newToast);
            if (!coroutineRunning)
            {
                StartCoroutine(ToastsQueueCoroutine());
            }
        }

        /// <summary>
        ///   Show a toast, if the toast is already showing a message, this message will be canceled
        /// </summary>
        public void ShowToast(string message, Sprite sprite, float duration)
        {
            ShowToast(message, sprite, duration, new Vector2(0.5F, 0.5F));
        }

        /// <summary>
        ///   Show a toast, if the toast is already showing a message, this message will be canceled
        /// </summary>
        public void ShowToast(string message, Sprite sprite, float duration, Vector2 fadeInOutTimes)
        {
            if (!showing)
            {
                showing = true;
                ToastStruct toast = new ToastStruct(message, sprite, duration, fadeInOutTimes);
                StartCoroutine(Toast(toast));
            }
        }

        private void Awake()
        {
            text = GetComponentInChildren<Text>();
            image = GetComponent<Image>();
            initImageAlpha = image.canvasRenderer.GetAlpha();
            initTextAlpha = text.canvasRenderer.GetAlpha();
        }

        private void Start()
        {
            SetVisible(false);
        }

        private void OnEnable()
        {
            SetVisible(false);
        }

        private IEnumerator ToastsQueueCoroutine()
        {
            coroutineRunning = true;
            while (toastsQueue.Count != 0)
            {
                if (!showing)
                {
                    showing = true;
                    ToastStruct toast = toastsQueue.Dequeue();
                    StartCoroutine(Toast(toast));
                }
                yield return new WaitForEndOfFrame();
            }
            coroutineRunning = false;
        }

        private IEnumerator Toast(ToastStruct toast)
        {
            text.text = toast.message;
            if (toast.sprite != null)
            {
                image.sprite = toast.sprite;
            }
            float imageAlpha = 0;
            float textAlpha = 0;
            float imageStride = initImageAlpha * (Time.fixedDeltaTime / toast.fadeInOutTimes.x);
            float textStride = initTextAlpha * (Time.fixedDeltaTime / toast.fadeInOutTimes.x);

            image.canvasRenderer.SetAlpha(imageAlpha);
            text.canvasRenderer.SetAlpha(textAlpha);
            bool textCompleted = false;
            bool imageCompleted = false;
            while (!(imageCompleted && textCompleted))
            {
                if (imageAlpha + imageStride < initImageAlpha)
                {
                    image.canvasRenderer.SetAlpha(imageAlpha);
                    imageAlpha += imageStride;
                }
                else
                {
                    image.canvasRenderer.SetAlpha(initImageAlpha);
                    imageCompleted = true;
                }
                if (textAlpha + textStride < initTextAlpha)
                {
                    text.canvasRenderer.SetAlpha(textAlpha);
                    textAlpha += textStride;
                }
                else
                {
                    text.canvasRenderer.SetAlpha(initTextAlpha);
                    textCompleted = true;
                }
                yield return new WaitForFixedUpdate();
            }
            SetVisible(true);
            yield return new WaitForSecondsRealtime(toast.duration);
            imageAlpha = initImageAlpha;
            textAlpha = initTextAlpha;
            image.canvasRenderer.SetAlpha(initImageAlpha);
            text.canvasRenderer.SetAlpha(initTextAlpha);
            textCompleted = false;
            imageCompleted = false;
            while (!(imageCompleted && textCompleted))
            {
                if (imageAlpha - imageStride > 0)
                {
                    image.canvasRenderer.SetAlpha(imageAlpha);
                    imageAlpha -= imageStride;
                }
                else
                {
                    image.canvasRenderer.SetAlpha(0);
                    imageCompleted = true;
                }
                if (textAlpha - textStride > 0)
                {
                    text.canvasRenderer.SetAlpha(textAlpha);
                    textAlpha -= textStride;
                }
                else
                {
                    text.canvasRenderer.SetAlpha(0);
                    textCompleted = true;
                }
                yield return new WaitForFixedUpdate();
            }
            SetVisible(false);
            showing = false;
        }

        private void SetVisible(bool state)
        {
            if (state)
            {
                image.canvasRenderer.SetAlpha(initImageAlpha);
                text.canvasRenderer.SetAlpha(initTextAlpha);
            }
            else
            {
                image.canvasRenderer.SetAlpha(0);
                text.canvasRenderer.SetAlpha(0);
            }
        }

        private struct ToastStruct
        {
            public Sprite sprite;
            public string message;
            public float duration;
            public Vector2 fadeInOutTimes;

            public ToastStruct(string message, Sprite sprite, float duration, Vector2 fadeInOutTimes)
            {
                this.sprite = sprite;
                this.message = message;
                this.duration = duration;
                this.fadeInOutTimes = fadeInOutTimes;
            }
        }
    }
}