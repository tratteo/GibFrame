//Copyright (c) matteo
//TFloatingJoystick.cs - com.tratteo.gibframe

using UnityEngine;
using UnityEngine.EventSystems;

namespace GibFrame.Joystick
{
    public class TFloatingJoystick : TJoystick
    {
        private Vector2 center = Vector2.zero;

        public override void OnDrag(PointerEventData eventData)
        {
            Vector2 direction = eventData.position - center;
            input = (direction.magnitude > background.sizeDelta.x / 2F) ? direction.normalized : direction / (background.sizeDelta.x / 2F);
            Clamp();
            handle.anchoredPosition = (input * background.sizeDelta.x / 2F) * handleLimit;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            background.gameObject.SetActive(true);
            background.position = eventData.position;
            handle.anchoredPosition = Vector2.zero;
            center = eventData.position;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            background.gameObject.SetActive(false);
            input = Vector2.zero;
        }

        private void Start()
        {
            background.gameObject.SetActive(false);
        }
    }
}