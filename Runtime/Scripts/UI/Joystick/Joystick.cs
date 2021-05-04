//Copyright (c) matteo
//TJoystick.cs - com.tratteo.gibframe

using UnityEngine;
using UnityEngine.EventSystems;

namespace GibFrame.Joystick
{
    public abstract class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        public enum JoystickMode { ALL_AXIS, VERTICAL, HORIZONTAL }

        [SerializeField] protected RectTransform handle;
        [SerializeField] protected RectTransform background;

        [Header("Options")]
        [SerializeField] protected JoystickMode mode;
        protected bool dragging = false;
        [SerializeField] private float handleLimit = 1F;
        [SerializeField] private float zeroThreshold = 0F;

        public float Horizontal { get; private set; }

        public float Vertical { get; private set; }

        public Vector2 Direction => new Vector2(Horizontal, Vertical);

        public Vector3 Direction3 => new Vector3(Horizontal, 0F, Vertical);

        public float Radius => handleLimit * background.sizeDelta.x / 2F;

        public virtual void OnDrag(PointerEventData eventData)
        {
            SetInput(eventData.position - GetCenter());
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            Horizontal = 0F;
            Vertical = 0F;
            dragging = false;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            dragging = true;
            handle.anchoredPosition = Vector3.zero;
        }

        protected abstract Vector2 GetCenter();

        protected void SetInput(Vector2 direction)
        {
            switch (mode)
            {
                case JoystickMode.VERTICAL:
                    direction.x = 0;
                    break;

                case JoystickMode.HORIZONTAL:
                    direction.y = 0;
                    break;
            }
            Vector2 clampedPos = direction.magnitude > Radius ? direction.normalized * Radius : direction;
            handle.anchoredPosition = clampedPos;
            if (direction.magnitude < Radius * zeroThreshold)
            {
                Horizontal = 0F;
                Vertical = 0F;
            }
            else
            {
                Horizontal = clampedPos.x / Radius;
                Vertical = clampedPos.y / Radius;
            }
        }

        private void OnEnable()
        {
            Vertical = 0F;
            Horizontal = 0F;
            handle.anchoredPosition = Vector2.zero;
        }
    }
}
