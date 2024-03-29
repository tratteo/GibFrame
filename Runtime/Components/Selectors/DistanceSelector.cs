using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GibFrame.Selectors
{
    public class DistanceSelector : DiscreteSelector
    {
        [Header("Distance based")]
        public DistanceSelectorParadigm paradigm;
        public float radius = 8F;

        private Collider[] buffer;

        public event Action<IEnumerable<Collider>> Detected = delegate { };

        protected override void Awake()
        {
            base.Awake();
            buffer = new Collider[BufferSize];
        }

        protected override IEnumerator SelectCoroutine()
        {
            var delay = new WaitForSeconds(updateInterval);
            yield return delay;
            while (Enabled)
            {
                Collider selected;
                if (NonAlloc)
                {
                    var amount = Physics.OverlapSphereNonAlloc(transform.position, radius, buffer, mask);
                    Detected?.Invoke(buffer.Take(amount).Where(c => IsGameObjectValid(c.gameObject)));
                    selected = ChooseNonAlloc(buffer, amount);
                    if (selected)
                    {
                        Select(selected.gameObject);
                    }
                    else
                    {
                        ResetSelection();
                    }
                }
                else
                {
                    buffer = Physics.OverlapSphere(transform.position, radius, mask);
                    Detected?.Invoke(buffer.Where(c => IsGameObjectValid(c.gameObject)));
                    selected = ChooseNonAlloc(buffer, buffer.Length);
                    if (selected)
                    {
                        Select(selected.gameObject);
                    }
                    else
                    {
                        ResetSelection();
                    }
                }
                yield return delay;
            }
        }

        private Collider ChooseNonAlloc(IEnumerable<Collider> array, int bufferMatch)
        {
            Collider selected = null;
            switch (paradigm)
            {
                case DistanceSelectorParadigm.DetectOnly:
                    return selected;

                case DistanceSelectorParadigm.Closest:
                    var min = float.MaxValue;
                    for (var i = 0; i < bufferMatch; i++)
                    {
                        var sqrDistance = Vector3.SqrMagnitude(array.ElementAt(i).transform.position - transform.position);
                        if (sqrDistance < min)
                        {
                            selected = array.ElementAt(i);
                            min = sqrDistance;
                        }
                    }
                    return selected;

                case DistanceSelectorParadigm.Farthest:
                    var max = float.MinValue;
                    for (var i = 0; i < bufferMatch; i++)
                    {
                        var sqrDistance = Vector3.SqrMagnitude(array.ElementAt(i).transform.position - transform.position);
                        if (sqrDistance > max)
                        {
                            selected = array.ElementAt(i);
                            max = sqrDistance;
                        }
                    }
                    return selected;

                default: return null;
            }
        }

        private void OnDrawGizmos()
        {
            if (DebugRender)
            {
                Gizmos.DrawWireSphere(transform.position, radius);
            }
        }
    }
}