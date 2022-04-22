// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.Joystick : FloatingJoystick.cs
//
// All Rights Reserved

using UnityEngine;
using UnityEngine.EventSystems;

namespace GibFrame.Joystick
{
    public class FloatingJoystick : Joystick
    {
        private Vector2 center = Vector2.zero;

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            background.gameObject.SetActive(true);
            background.position = eventData.position;
            center = eventData.position;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            background.gameObject.SetActive(false);
        }

        protected override Vector2 GetCenter() => center;

        private void Start()
        {
            background.gameObject.SetActive(false);
        }
    }
}