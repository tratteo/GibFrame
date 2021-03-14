//Copyright (c) matteo
//TFixedJoystick.cs - com.tratteo.gibframe

using UnityEngine;
using UnityEngine.EventSystems;

namespace GibFrame.Joystick
{
    public class TFixedJoystick : TJoystick
    {
        private Vector2 postition = Vector2.zero;

        private Camera virtualCam = new Camera();

        public override void OnDrag(PointerEventData eventData)
        {
            Vector2 direction = eventData.position - postition;
            input = (direction.magnitude > background.sizeDelta.x / 2F) ? direction.normalized : direction / (background.sizeDelta.x / 2F);
            Clamp();
            handle.anchoredPosition = (input * background.sizeDelta.x / 2F) * handleLimit;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
        }

        private void Start()
        {
            postition = RectTransformUtility.WorldToScreenPoint(virtualCam, background.position);
        }
    }
}