// Copyright (c) 2020 Matteo Beltrame

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
    public class GButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
        public Sprite pressedSprite;
        public bool colorPressEffect;
        public Color32 pressedColor;
        public bool resizeOnPress;
        public Vector2 pressedScaleMultiplier;
        public Color32 defaultColor;
        public Image image;
        public UnityEvent onPressed;
        public UnityEvent onReleased;
        private Sprite unpressedSprite;
        private List<AbstractCallback> OnReleaseCallbacks;
        private List<AbstractCallback> OnPressedCallbacks;
        private EventTrigger.Entry pointerDown;
        private EventTrigger.Entry pointerUp;
        private bool canReleaseExecute = false;
        private bool clicked = false;

        public void AddOnPressedCallback(AbstractCallback Callback)
        {
            OnPressedCallbacks.Add(Callback);
        }

        public void AddOnReleasedCallback(AbstractCallback Callback)
        {
            OnReleaseCallbacks.Add(Callback);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            canReleaseExecute = true;
            if (clicked)
            {
                PressUI();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            canReleaseExecute = false;
            ResetUI();
        }

        protected virtual void Awake()
        {
            OnReleaseCallbacks = new List<AbstractCallback>();
            OnPressedCallbacks = new List<AbstractCallback>();
            image = GetComponentInChildren<Image>();
            unpressedSprite = image.sprite;

            defaultColor = image.color;

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
            clicked = true;
            PressUI();
            onPressed.Invoke();
            foreach (AbstractCallback callback in OnPressedCallbacks)
            {
                callback.Invoke();
            }
        }

        private void Released()
        {
            ResetUI();
            clicked = false;
            foreach (AbstractCallback callback in OnReleaseCallbacks)
            {
                callback.Invoke();
            }
            if (canReleaseExecute)
            {
                onReleased.Invoke();
            }
        }

        private void ResetUI()
        {
            if (colorPressEffect)
            {
                image.color = defaultColor;
            }
            if (resizeOnPress)
            {
                image.rectTransform.localScale = Vector2.one;
            }
            image.sprite = unpressedSprite;
        }

        private void PressUI()
        {
            if (colorPressEffect)
            {
                image.color = pressedColor;
            }
            if (resizeOnPress)
            {
                Vector2 newScale = new Vector2(image.rectTransform.localScale.x * pressedScaleMultiplier.x, image.rectTransform.localScale.y * pressedScaleMultiplier.y);
                image.rectTransform.localScale = newScale;
            }
            if (pressedSprite != null)
            {
                image.sprite = pressedSprite;
            }
        }
    }
}