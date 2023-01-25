using System.Collections;
using UnityEngine;

namespace GibFrame.Selectors
{
    public class RaycastSelector2D : DiscreteSelector
    {
        private Vector3 direction;
        private bool directionOverride = false;
        private Vector3 offset;
        private RaycastHit2D[] hitsBuffer;

        public void OverrideDirection(Vector3 direction)
        {
            this.direction = direction;
            directionOverride = true;
        }

        public void ResetDirection()
        {
            directionOverride = false;
        }

        public void StartPointOffset(Vector3 offset)
        {
            this.offset = offset;
        }

        protected override void Start()
        {
            base.Start();
            hitsBuffer = new RaycastHit2D[BufferSize];
        }

        protected override IEnumerator SelectCoroutine()
        {
            var delay = new WaitForSecondsRealtime(updateInterval);
            yield return delay;
            while (Enabled)
            {
                var dir = directionOverride ? direction : transform.right;
                if (NonAlloc)
                {
                    var ray = new Ray(transform.position + offset, dir);
                    var count = Physics2D.RaycastNonAlloc(ray.origin, ray.direction, hitsBuffer, float.MaxValue, mask);
                    if (count > 0)
                    {
                        var obj = hitsBuffer[0].collider.gameObject;
                        if (IsGameObjectValid(obj))
                        {
                            Select(obj);
                        }
                    }
                    else
                    {
                        ResetSelection();
                    }
                }
                else
                {
                    var hit = Physics2D.Raycast(transform.position + offset, dir, float.MaxValue, mask);
                    if (hit.collider)
                    {
                        if (IsGameObjectValid(hit.collider.gameObject))
                        {
                            Select(hit.collider.gameObject);
                        }
                        else
                        {
                            ResetSelection();
                        }
                    }
                    else
                    {
                        ResetSelection();
                    }
                }

                yield return delay;
            }
        }

        private void OnDrawGizmos()
        {
            if (DebugRender)
            {
                var dir = directionOverride ? direction : transform.right;
                Gizmos.DrawRay(transform.position, dir * 2F);
            }
        }
    }
}