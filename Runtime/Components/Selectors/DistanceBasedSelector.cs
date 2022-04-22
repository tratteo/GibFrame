// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : DistanceBasedSelector.cs
//
// All Rights Reserved

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GibFrame.Selectors
{
    public class DistanceBasedSelector : DiscreteSelector
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
        private Collider[] buffer;

        protected override void Awake()
        {
            base.Awake();
            buffer = new Collider[bufferSize];
        }

        protected override IEnumerator SelectCoroutine()
        {
            var delay = new WaitForSeconds(updateInterval);
            yield return delay;
            while (Enabled)
            {
                Collider selected;
                if (nonAlloc)
                {
                    var amount = Physics.OverlapSphereNonAlloc(transform.position, radius, buffer, mask);
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
                    buffer = Physics.OverlapSphere(transform.position, radius, mask);

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

        private Collider ChooseNonAlloc(IEnumerable<Collider> array, int bufferMatch)
        {
            Collider selected = null;
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
