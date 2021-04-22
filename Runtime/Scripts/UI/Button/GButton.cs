//Copyright (c) matteo
//GButton.cs - com.tratteo.gibframe

using System.Collections.Generic;
using GibFrame.Extensions;
using GibFrame.Patterns;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GibFrame.UI
{
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
        public bool inheritCallbackEvents = false;
        public bool callbackOnlyOnPointerInside = true;
        private Sprite unpressedSprite;

        private List<AbstractCallback> OnReleaseCallbacks;
        private List<AbstractCallback> OnPressedCallbacks;
        private List<AbstractCallback> OnPointerEnterCallbacks;
        private List<AbstractCallback> OnPointerExitCallbacks;
        private List<AbstractCallback> OnCancelCallbacks;

        private EventTrigger.Entry pointerDown;
        private EventTrigger.Entry pointerUp;
        private bool canReleaseExecute = false;
        private bool clicked = false;
        private GButton[] childButtons;

        public void AddOnCancelCallback(AbstractCallback Callback)
        {
            OnCancelCallbacks.Add(Callback);
        }

        public void AddOnPressedCallback(AbstractCallback Callback)
        {
            OnPressedCallbacks.Add(Callback);
        }

        public void AddOnReleasedCallback(AbstractCallback Callback)
        {
            OnReleaseCallbacks.Add(Callback);
        }

        public void AddOnPointerEnterCallback(AbstractCallback callback)
        {
            OnPointerEnterCallbacks.Add(callback);
        }

        public void AddOnPointerExitCallback(AbstractCallback callback)
        {
            OnPointerExitCallbacks.Add(callback);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            foreach (AbstractCallback callback in OnPointerEnterCallbacks)
            {
                callback.Invoke();
            }

            if (callbackOnlyOnPointerInside)
            {
                canReleaseExecute = true;
                if (clicked)
                {
                    PressUI();
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            foreach (AbstractCallback callback in OnPointerExitCallbacks)
            {
                callback.Invoke();
            }
            if (callbackOnlyOnPointerInside)
            {
                canReleaseExecute = false;
                ResetUI();
            }
        }

        protected virtual void Awake()
        {
            OnReleaseCallbacks = new List<AbstractCallback>();
            OnPressedCallbacks = new List<AbstractCallback>();
            OnPointerExitCallbacks = new List<AbstractCallback>();
            OnPointerEnterCallbacks = new List<AbstractCallback>();
            OnCancelCallbacks = new List<AbstractCallback>();
            childButtons = GetComponentsInChildren<GButton>(true);
            childButtons = childButtons.GetPredicatesMatchingObjects((b) => b.inheritCallbackEvents && !b.gameObject.Equals(gameObject));
            image = GetComponentInChildren<Image>();
            unpressedSprite = image.sprite;

            defaultColor = image.color;

            EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

            pointerDown = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            pointerDown.callback.AddListener((e) => Pressed());

            pointerUp = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp
            };
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
            foreach (GButton child in childButtons)
            {
                child.onPressed?.Invoke();
            }
        }

        private void Released()
        {
            ResetUI();
            clicked = false;

            if (canReleaseExecute || !callbackOnlyOnPointerInside)
            {
                onReleased.Invoke();
                foreach (GButton child in childButtons)
                {
                    child.onReleased?.Invoke();
                }
                foreach (AbstractCallback callback in OnReleaseCallbacks)
                {
                    callback.Invoke();
                }
            }
            else
            {
                foreach (AbstractCallback callback in OnCancelCallbacks)
                {
                    callback.Invoke();
                }
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
            foreach (GButton child in childButtons)
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
            if (resizeOnPress)
            {
                Vector2 newScale = new Vector2(image.rectTransform.localScale.x * pressedScaleMultiplier.x, image.rectTransform.localScale.y * pressedScaleMultiplier.y);
                image.rectTransform.localScale = newScale;
            }
            if (pressedSprite != null)
            {
                image.sprite = pressedSprite;
            }
            foreach (GButton child in childButtons)
            {
                child.PressUI();
            }
        }
    }
}
