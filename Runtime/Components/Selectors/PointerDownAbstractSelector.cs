using UnityEngine;
using UnityEngine.Events;

namespace GibFrame.Selectors
{
    public abstract class PointerDownAbstractSelector : Selector
    {
        public enum InputType
        { Mouse, Touch, Both }

        [Header("Pointer based")]
        public InputType input;

        [SerializeField] private UnityEvent OnMissed;

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

                    if (touch.phase == TouchPhase.Ended)
                    {
                        var hit = HitObject(touch.position);
                        if (hit && IsGameObjectValid(hit))
                        {
                            Select(hit);
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
                    var hit = HitObject(Input.mousePosition);
                    if (hit && IsGameObjectValid(hit))
                    {
                        Select(hit);
                    }
                    else
                    {
                        ResetSelection();
                        OnMissed?.Invoke();
                    }
                }
            }
        }

        protected abstract GameObject HitObject(Vector2 screenPoint);
    }
}