//Copyright (c) matteo
//PointerDownBasedSelector.cs - com.tratteo.gibframe

using GibFrame.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace GibFrame.Selectors
{
    public class PointerDownBasedSelector : Selector
    {
        public enum InputType { MOUSE, TOUCH, BOTH }

        [SerializeField] private InputType input;
        private Camera mainCamera;
        [SerializeField] private UnityEvent OnMissed;

        protected void Update()
        {
            if (input == InputType.TOUCH || input == InputType.BOTH)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    Ray ray = mainCamera.ScreenPointToRay(touch.position);
                    if (UnityUtils.IsAnyPointerOverGameObject()) return;
                    if (touch.phase == TouchPhase.Ended)
                    {
                        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, mask))
                        {
                            if (IsColliderValid(hit.collider))
                            {
                                Select(hit.collider);
                            }
                        }
                        else
                        {
                            ResetSelection();
                            OnMissed?.Invoke();
                        }
                    }
                }
            }

            if (input == InputType.MOUSE || input == InputType.BOTH)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (UnityUtils.IsAnyPointerOverGameObject()) return;
                    if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, mask))
                    {
                        if (IsColliderValid(hit.collider))
                        {
                            Select(hit.collider);
                        }
                    }
                    else
                    {
                        ResetSelection();
                        OnMissed?.Invoke();
                    }
                }
            }
        }

        private void Start()
        {
            mainCamera = Camera.main;
        }
    }
}
