// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : PointerDownBasedSelector.cs
//
// All Rights Reserved

using UnityEngine;
using UnityEngine.Events;

namespace GibFrame.Selectors
{
    public class PointerDownBasedSelector : Selector
    {
        public enum InputType
        { Mouse, Touch, Both }

        [Header("Pointer based")]
        public InputType input;
        [SerializeField] private UnityEvent OnMissed;
        [SerializeField] private Camera targetCamera = null;

        protected void Update()
        {
            if (!Enabled)
            {
                return;
            }

            if (input is InputType.Touch or InputType.Both)
            {
                if (Input.touchCount > 0)
                {
                    var touch = Input.GetTouch(0);
                    var ray = GetCamera().ScreenPointToRay(touch.position);
                    if (UnityUtils.IsAnyPointerOverGameObject())
                    {
                        return;
                    }

                    if (touch.phase == TouchPhase.Ended)
                    {
                        if (Physics.Raycast(ray, out var hit, float.MaxValue, mask))
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

            if (input is InputType.Mouse or InputType.Both)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    var ray = GetCamera().ScreenPointToRay(Input.mousePosition);
                    if (UnityUtils.IsAnyPointerOverGameObject())
                    {
                        return;
                    }

                    if (Physics.Raycast(ray, out var hit, float.MaxValue, mask))
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

        private Camera GetCamera()
        {
            targetCamera = targetCamera != null ? targetCamera : Camera.main;
            return targetCamera;
        }
    }
}
