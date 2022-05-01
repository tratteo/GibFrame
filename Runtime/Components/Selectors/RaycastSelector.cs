// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : RaycastBasedSelector.cs
//
// All Rights Reserved

using System.Collections;
using UnityEngine;

namespace GibFrame.Selectors
{
    public class RaycastSelector : DiscreteSelector
    {
        private Vector3 direction;
        private bool directionOverride = false;
        private Vector3 offset;
        private RaycastHit[] hitsBuffer;

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
            hitsBuffer = new RaycastHit[BufferSize];
        }

        protected override IEnumerator SelectCoroutine()
        {
            var delay = new WaitForSecondsRealtime(updateInterval);
            yield return delay;
            while (Enabled)
            {
                var dir = directionOverride ? direction : transform.forward;

                if (NonAlloc)
                {
                    var ray = new Ray(transform.position + offset, dir);
                    var count = Physics.RaycastNonAlloc(ray, hitsBuffer, float.MaxValue, mask);
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
                    if (Physics.Raycast(transform.position + offset, dir, out var hit, float.MaxValue, mask))
                    {
                        var obj = hit.collider.gameObject;
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

                yield return delay;
            }
        }

        private void OnDrawGizmos()
        {
            if (DebugRender)
            {
                var dir = directionOverride ? direction : transform.forward;
                Gizmos.DrawRay(transform.position, dir * 2F);
            }
        }
    }
}
