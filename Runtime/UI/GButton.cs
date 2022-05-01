// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.UI : GButton.cs
//
// All Rights Reserved

using System;
using GibFrame.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GibFrame.UI
{
    public class GButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
        [SerializeField] private Sprite pressedSprite;

        [SerializeField] private bool colorPressEffect;
        [SerializeField] private Color32 pressedColor;

        [SerializeField] private bool pressedSizeEffect;
        [SerializeField] private Vector2 pressedScaleMultiplier = new Vector2(1, 1);

        [SerializeField] private bool enableLongPress;
        [SerializeField] private UnityEvent onLongPressed;
        [SerializeField] private int longPressDelay;
        [SerializeField] private bool resetOnFire = true;
        [SerializeField] private UnityEvent onPressed;
        [SerializeField] private UnityEvent onReleased;

        [SerializeField] private bool inheritCallbackEvents = false;
        [SerializeField] private bool callbackOnlyOnPointerInside = true;

        private Sprite unpressedSprite;
        private Image image;
        private Color32 defaultColor;

        private EventTrigger.Entry pointerDown;
        private EventTrigger.Entry pointerUp;
        private bool pointerInside = false;
        private bool clicked = false;
        private GButton[] childButtons;
        private float currentPressTime = 0F;

        public event Action LongPressed = delegate { };

        public event Action PointerExit = delegate { };

        public event Action PointerEnter = delegate { };

        public event Action Pressed = delegate { };

        public event Action Released = delegate { };

        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnter.Invoke();

            if (callbackOnlyOnPointerInside)
            {
                pointerInside = true;
                if (clicked)
                {
                    PressUI();
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExit.Invoke();
            if (callbackOnlyOnPointerInside)
            {
                pointerInside = false;
                ResetUI();
            }
            currentPressTime = 0F;
        }

        protected virtual void Awake()
        {
            childButtons = GetComponentsInChildren<GButton>(true);
            childButtons = childButtons.FindAll((b) => b.inheritCallbackEvents && !b.gameObject.Equals(gameObject)).ToArray();
            image = GetComponentInChildren<Image>();
            unpressedSprite = image.sprite;

            defaultColor = image.color;

            var trigger = gameObject.AddComponent<EventTrigger>();

            pointerDown = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            pointerDown.callback.AddListener((e) => OnPressed());

            pointerUp = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp
            };
            pointerUp.callback.AddListener((e) => OnReleased());

            trigger.triggers.Add(pointerDown);
            trigger.triggers.Add(pointerUp);
        }

        private void Update()
        {
            if (enableLongPress && pointerInside && (!resetOnFire || clicked))
            {
                currentPressTime += Time.unscaledDeltaTime * 1000F;
                if (currentPressTime >= longPressDelay)
                {
                    if (pointerInside || !callbackOnlyOnPointerInside)
                    {
                        onLongPressed.Invoke();
                        LongPressed.Invoke();
                        currentPressTime = 0;
                        OnReleased(true);
                    }
                }
            }
        }

        private void OnPressed()
        {
            clicked = true;
            PressUI();
            onPressed.Invoke();
            Pressed.Invoke();
            foreach (var child in childButtons)
            {
                child.onPressed?.Invoke();
            }
        }

        private void OnReleased(bool fromLongPress = false)
        {
            if (!fromLongPress || resetOnFire)
            {
                ResetUI();
            }
            clicked = false;
            currentPressTime = 0F;
            if (pointerInside || !callbackOnlyOnPointerInside)
            {
                onReleased.Invoke();
                foreach (var child in childButtons)
                {
                    child.onReleased?.Invoke();
                }
                Released.Invoke();
            }
        }

        private void ResetUI()
        {
            if (colorPressEffect)
            {
                image.color = defaultColor;
            }
            if (pressedSizeEffect)
            {
                image.rectTransform.localScale = Vector2.one;
            }
            image.sprite = unpressedSprite;
            foreach (var child in childButtons)
            {
                child.ResetUI();
            }
        }

        private void PressUI()
        {
            if (colorPressEffect)
            {
                image.color = pressedColor;
            }
            if (pressedSizeEffect)
            {
                var newScale = new Vector2(image.rectTransform.localScale.x * pressedScaleMultiplier.x, image.rectTransform.localScale.y * pressedScaleMultiplier.y);
                image.rectTransform.localScale = newScale;
            }
            if (pressedSprite != null)
            {
                image.sprite = pressedSprite;
            }

            foreach (var child in childButtons)
            {
                child.ResetUI();
            }
        }
    }
}
