using UnityEngine;

namespace GibFrame.Selectors
{
    public class PointerDownSelector : PointerDownAbstractSelector
    {
        [SerializeField] private Camera targetCamera = null;

        private RaycastHit[] hitsBuffer;

        protected override GameObject HitObject(Vector2 screenPos)
        {
            if (Gib.IsAnyPointerOverGameObject())
            {
                return null;
            }

            var ray = GetCamera().ScreenPointToRay(screenPos);
            if (NonAlloc)
            {
                var hit = Physics.RaycastNonAlloc(ray.origin, ray.direction, hitsBuffer, float.MaxValue, mask);
                if (hit > 0)
                {
                    return hitsBuffer[0].collider.gameObject;
                }
            }
            else
            {
                if (Physics.Raycast(ray.origin, ray.direction, out var hit, float.MaxValue, mask))
                {
                    return hit.collider.gameObject;
                }
            }
            return null;
        }

        protected override void Start()
        {
            base.Start();
            hitsBuffer = new RaycastHit[BufferSize];
        }

        private Camera GetCamera()
        {
            targetCamera = targetCamera != null ? targetCamera : Camera.main;
            return targetCamera;
        }
    }
}