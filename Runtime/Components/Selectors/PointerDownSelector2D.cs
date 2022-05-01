using UnityEngine;

namespace GibFrame.Selectors
{
    public class PointerDownSelector2D : PointerDownAbstractSelector
    {
        [SerializeField] private Camera targetCamera = null;
        private RaycastHit2D[] hitsBuffer;

        protected override GameObject HitObject(Vector2 screenPos)
        {
            if (UnityUtils.IsAnyPointerOverGameObject())
            {
                return null;
            }

            var ray = GetCamera().ScreenPointToRay(screenPos);
            if (NonAlloc)
            {
                var hit = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, hitsBuffer, float.MaxValue, mask);
                if (hit > 0)
                {
                    return hitsBuffer[0].collider.gameObject;
                }
            }
            else
            {
                var hit = Physics2D.Raycast(ray.origin, ray.direction, float.MaxValue, mask);
                if (hit.collider)
                {
                    return hit.collider.gameObject;
                }
            }
            return null;
        }

        protected override void Start()
        {
            base.Start();
            hitsBuffer = new RaycastHit2D[BufferSize];
        }

        private Camera GetCamera()
        {
            targetCamera = targetCamera != null ? targetCamera : Camera.main;
            return targetCamera;
        }
    }
}
