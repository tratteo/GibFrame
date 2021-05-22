//Copyright (c) matteo
//FixedJoystick.cs - com.tratteo.gibframe

using UnityEngine;
using UnityEngine.EventSystems;

namespace GibFrame.Joystick
{
    public class FixedJoystick : Joystick
    {
        private Vector2 postition = Vector2.zero;
        [SerializeField] private bool smoothTransitions = false;
        [SerializeField] private float transitionsSpeed = 0.2F;

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (!smoothTransitions)
            {
                handle.anchoredPosition = Vector3.zero;
            }
        }

        protected override Vector2 GetCenter() => postition;

        private void Update()
        {
            if (!dragging && smoothTransitions)
            {
                handle.localPosition = Vector2.Lerp(handle.localPosition, Vector2.zero, transitionsSpeed);
            }
        }

        private void Awake()
        {
            postition = background.position;
        }
    }
}