// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : RaycastBasedSelector.cs
//
// All Rights Reserved

using System.Collections;
using UnityEngine;

namespace GibFrame
{
    public class RaycastBasedSelector : DiscreteSelector
    {
        private Vector3 direction;
        private bool directionOverride = false;
        private Vector3 offset;

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

        protected override IEnumerator SelectCoroutine()
        {
            WaitForSecondsRealtime Delay = new WaitForSecondsRealtime(updateInterval);
            yield return Delay;
            while (true)
            {
                if (Active)
                {
                    Vector3 dir = directionOverride ? direction : transform.forward;
                    if (Physics.Raycast(transform.position + offset, dir, out RaycastHit hit, float.MaxValue, Mask))
                    {
                        if (IsColliderValid(hit.collider))
                        {
                            Select(hit.collider);
                        }
                    }
                    else
                    {
                        ResetSelection();
                    }
                }

                yield return Delay;
            }
        }
    }
}