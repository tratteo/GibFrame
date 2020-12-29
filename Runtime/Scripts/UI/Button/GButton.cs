﻿// Copyright (c) 2020 Matteo Beltrame

using System;
using System.Collections.Generic;
using GibFrame.Utils.Callbacks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GibFrame.UI
{
    /// <summary>
    ///   Button press utility. Attach this component to a UI image.
    /// </summary>
    public class GButton : MonoBehaviour
    {
        public bool colorPressEffect;
        public Color32 pressedColor;
        public bool resizeOnPress;
        public Vector2 pressedScaleMultiplier;

        public Color32 defaultColor;
        public Image sprite;

        public UnityEvent onPressed;
        public UnityEvent onReleased;
        private List<AbstractCallback> OnReleaseCallbacks;
        private List<AbstractCallback> OnPressedCallbacks;
        private EventTrigger.Entry pointerDown;
        private EventTrigger.Entry pointerUp;

        public void AddOnPressedCallback(AbstractCallback Callback)
        {
            OnPressedCallbacks.Add(Callback);
        }

        public void AddOnReleasedCallback(AbstractCallback Callback)
        {
            OnReleaseCallbacks.Add(Callback);
        }

        protected virtual void Awake()
        {
            OnReleaseCallbacks = new List<AbstractCallback>();
            OnPressedCallbacks = new List<AbstractCallback>();
            sprite = GetComponentInChildren<Image>();

            defaultColor = sprite.color;

            EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

            pointerDown = new EventTrigger.Entry();
            pointerDown.eventID = EventTriggerType.PointerDown;
            pointerDown.callback.AddListener((e) => Pressed());

            pointerUp = new EventTrigger.Entry();
            pointerUp.eventID = EventTriggerType.PointerUp;
            pointerUp.callback.AddListener((e) => Released());

            trigger.triggers.Add(pointerDown);
            trigger.triggers.Add(pointerUp);
        }

        private void Pressed()
        {
            if (colorPressEffect)
            {
                sprite.color = pressedColor;
            }
            if (resizeOnPress)
            {
                Vector2 newScale = new Vector2(sprite.rectTransform.localScale.x * pressedScaleMultiplier.x, sprite.rectTransform.localScale.y * pressedScaleMultiplier.y);
                sprite.rectTransform.localScale = newScale;
            }
            onPressed.Invoke();
            foreach (AbstractCallback callback in OnPressedCallbacks)
            {
                callback.Invoke();
            }
        }

        private void Released()
        {
            if (colorPressEffect)
            {
                sprite.color = defaultColor;
            }
            if (resizeOnPress)
            {
                sprite.rectTransform.localScale = Vector2.one;
            }
            onReleased.Invoke();
            foreach (AbstractCallback callback in OnReleaseCallbacks)
            {
                callback.Invoke();
            }
        }
    }
}