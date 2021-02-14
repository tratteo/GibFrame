// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.Joystick : TJoystick.cs
//
// All Rights Reserved

using UnityEngine;
using UnityEngine.EventSystems;

namespace GibFrame.Joystick
{
    public class TJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        public enum JoystickMode { ALL_AXIS, VERTICAL, HORIZONTAL }

        [Header("Options")]
        [Range(0f, 2f)] public float handleLimit = 1f;
        [Header("Components")]
        public RectTransform background;
        public RectTransform handle;
        protected Vector2 input = Vector2.zero;
        [SerializeField] protected JoystickMode mode;

        public float Horizontal { get { return input.x; } }

        public float Vertical { get { return input.y; } }

        public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

        public virtual void OnDrag(PointerEventData eventData)
        {
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
        }

        protected void Clamp()
        {
            switch (mode)
            {
                case JoystickMode.VERTICAL:
                    input.y = 0;
                    break;

                case JoystickMode.HORIZONTAL:
                    input.x = 0;
                    break;
            }
        }
    }
}