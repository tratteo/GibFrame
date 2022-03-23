// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : DistanceBasedSelector.cs
//
// All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GibFrame
{
    public class DistanceBasedSelector2D : DiscreteSelector2D
    {
        public enum Paradigm
        { Closest, Farthest }

        [Header("Distance based")]
        public Paradigm paradigm;
        public float radius = 8F;
        [Header("Advanced")]
        [SerializeField] private bool nonAlloc = false;
        [Tooltip("Define the maximum buffer size when using the non alloc option")]
        [SerializeField] private int bufferSize = 16;
        private Collider2D[] buffer;

        protected override void Awake()
        {
            base.Awake();
            buffer = new Collider2D[bufferSize];
        }

        protected override IEnumerator SelectCoroutine()
        {
            var delay = new WaitForSeconds(updateInterval);
            yield return delay;
            while (Enabled)
            {
                Collider2D selected;
                if (nonAlloc)
                {
                    var amount = Physics2D.OverlapCircleNonAlloc(transform.position, radius, buffer, mask);
                    selected = ChooseNonAlloc(buffer, amount);
                    if (selected)
                    {
                        Select(selected);
                    }
                    else
                    {
                        ResetSelection();
                    }
                }
                else
                {
                    buffer = Physics2D.OverlapCircleAll(transform.position, radius, mask);

                    selected = ChooseNonAlloc(buffer, buffer.Length);
                    if (selected)
                    {
                        Select(selected);
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
                case Paradigm.Closest:
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

                case Paradigm.Farthest:
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
