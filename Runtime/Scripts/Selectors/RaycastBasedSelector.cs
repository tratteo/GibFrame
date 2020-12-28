using System.Collections;
using UnityEngine;

namespace GibFrame.Selectors
{
    public class RaycastBasedSelector : DiscreteSelector
    {
        private bool directionOverride = false;
        private Vector3 offset;
        private Vector3 direction;

        public void StartPointOffset(Vector3 offset)
        {
            this.offset = offset;
        }

        public void OverrideDirection(Vector3 direction)
        {
            this.direction = direction;
            directionOverride = true;
        }

        public void ResetDirection()
        {
            directionOverride = false;
        }

        protected override IEnumerator SelectCoroutine()
        {
            yield return new WaitForSecondsRealtime(updateInterval);
            while (true)
            {
                if (Active)
                {
                    Vector3 dir = directionOverride ? direction : transform.forward;
                    //Debug.DrawRay(start, dir * 1000F, Color.red, 0.25F);
                    if (Physics.Raycast(transform.position + offset, dir, out RaycastHit hit, float.MaxValue))
                    {
                        if (hit.collider.CompareTag(selectableTag))
                        {
                            if (ColliderSatisfiesPredicates(hit.collider))
                            {
                                Select(hit.collider);
                            }
                        }
                    }
                    else
                    {
                        ResetSelection();
                    }
                }

                yield return new WaitForSecondsRealtime(updateInterval);
            }
        }
    }
}