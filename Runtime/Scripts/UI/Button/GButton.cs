//Copyright (c) matteo
//GButton.cs - com.tratteo.gibframe

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GibFrame.UI
{
    public class GButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
        public bool enableLongPress;
        public int longPressDelay;
        public UnityEvent onLongPressed;
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

        private CallbackEvent OnReleaseEvent;
        private CallbackEvent OnPressedEvent;
        private CallbackEvent OnPointerEnterEvent;
        private CallbackEvent OnPointerExitEvent;
        private CallbackEvent OnCancelEvent;
        private CallbackEvent OnLongPressEvent;

        private EventTrigger.Entry pointerDown;
        private EventTrigger.Entry pointerUp;
        private bool canReleaseExecute = false;
        private bool clicked = false;
        private GButton[] childButtons;
        private float currentPressTime = 0F;

        public void AddOnCancelCallback(AbstractCallback Callback)

        {
            OnCancelEvent.Subscribe(Callback);
        }

        public void AddOnPressedCallback(AbstractCallback Callback)
        {
            OnPressedEvent.Subscribe(Callback);
        }

        public void AddOnReleasedCallback(AbstractCallback Callback)
        {
            OnReleaseEvent.Subscribe(Callback);
        }

        public void AddOnPointerEnterCallback(AbstractCallback callback)
        {
            OnPointerEnterEvent.Subscribe(callback);
        }

        public void AddOnPointerExitCallback(AbstractCallback callback)
        {
            OnPointerExitEvent.Subscribe(callback);
        }

        public void AddOnLongPressedCallback(AbstractCallback callback)
        {
            OnLongPressEvent.Subscribe(callback);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterEvent.Invoke();

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
            OnPointerExitEvent.Invoke();
            if (callbackOnlyOnPointerInside)
            {
                canReleaseExecute = false;
                ResetUI();
            }
        }

        protected virtual void Awake()
        {
            OnReleaseEvent = new CallbackEvent();
            OnPressedEvent = new CallbackEvent();
            OnPointerExitEvent = new CallbackEvent();
            OnPointerEnterEvent = new CallbackEvent();
            OnCancelEvent = new CallbackEvent();
            OnLongPressEvent = new CallbackEvent();

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

        private void Update()
        {
            if (enableLongPress && clicked)
            {
                currentPressTime += Time.unscaledDeltaTime * 1000F;
                if (currentPressTime >= longPressDelay)
                {
                    onLongPressed.Invoke();
                    OnLongPressEvent.Invoke();
                    currentPressTime = 0;
                    Released();
                }
            }
        }

        private void Pressed()
        {
            clicked = true;
            PressUI();
            onPressed.Invoke();
            OnPressedEvent.Invoke();
            childButtons.ForEach(c => c.onPressed?.Invoke());
        }

        private void Released()
        {
            ResetUI();
            clicked = false;
            currentPressTime = 0F;
            if (canReleaseExecute || !callbackOnlyOnPointerInside)
            {
                onReleased.Invoke();
                childButtons.ForEach(c => c.onReleased?.Invoke());
                OnReleaseEvent.Invoke();
            }
            else
            {
                OnCancelEvent.Invoke();
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

            childButtons.ForEach(b => b.ResetUI());
        }
    }
}