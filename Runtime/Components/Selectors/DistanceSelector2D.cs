using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GibFrame.Selectors
{
    public class DistanceSelector2D : DiscreteSelector
    {
        [Header("Distance based")]
        public DistanceSelectorParadigm paradigm;
        public float radius = 8F;

        private Collider2D[] buffer;

        public event Action<IEnumerable<Collider2D>> Detected = delegate { };

        protected override void Awake()
        {
            base.Awake();
            buffer = new Collider2D[BufferSize];
        }

        protected override IEnumerator SelectCoroutine()
        {
            var delay = new WaitForSeconds(updateInterval);
            yield return delay;
            while (Enabled)
            {
                Collider2D selected;
                if (NonAlloc)
                {
                    var amount = Physics2D.OverlapCircleNonAlloc(transform.position, radius, buffer, mask);
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
                    buffer = Physics2D.OverlapCircleAll(transform.position, radius, mask);
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

        private Collider2D ChooseNonAlloc(IEnumerable<Collider2D> array, int bufferMatch)
        {
            Collider2D selected = null;
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