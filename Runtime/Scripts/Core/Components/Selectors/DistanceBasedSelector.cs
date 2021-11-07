// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : DistanceBasedSelector.cs
//
// All Rights Reserved

using System.Collections;
using UnityEngine;

namespace GibFrame
{
    public class DistanceBasedSelector : DiscreteSelector
    {
        public enum Paradigm { CLOSEST, FARTHEST }

        [Header("Distance based")]
        [SerializeField] private Paradigm paradigm;
        [SerializeField] private float radius = 8F;
        [Tooltip("Use the non alloc version for detection")]
        [SerializeField] private bool nonAlloc = false;
        [Tooltip("Define the maximum buffer size when using the non alloc option")]
        [SerializeField] private int bufferSize = 16;
        [Header("Debug")]
        [SerializeField] private bool debugRender = false;

        public float Radius { get => radius; set => radius = value; }

        public bool NonAlloc { get => nonAlloc; set => nonAlloc = value; }

        public int BufferSize { get => bufferSize; set => bufferSize = value; }

        public Paradigm DistanceParadigm { get => paradigm; set => paradigm = value; }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override IEnumerator SelectCoroutine()
        {
            Collider[] colliders = null;
            if (nonAlloc)
            {
                colliders = new Collider[bufferSize];
            }
            WaitForSeconds Delay = new WaitForSeconds(updateInterval);
            yield return Delay;
            while (true)
            {
                if (Active)
                {
                    Collider selected;
                    if (nonAlloc)
                    {
                        Physics.OverlapSphereNonAlloc(transform.position, radius, colliders, Mask);
                        selected = Choose(colliders);
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
                        colliders = Physics.OverlapSphere(transform.position, radius, Mask);
                        selected = Choose(colliders);
                        if (selected)
                        {
                            Select(selected);
                        }
                        else
                        {
                            ResetSelection();
                        }
                    }
                    yield return Delay;
                }
                yield return null;
            }
        }

        private Collider Choose(Collider[] rawArr)
        {
            rawArr = rawArr.GetPredicatesMatchingObjects((c) => IsColliderValid(c));
            if (rawArr.Length > 0)
            {
                switch (paradigm)
                {
                    case Paradigm.CLOSEST:
                        return rawArr.GetMin(c => Vector3.SqrMagnitude(c.transform.position - transform.position));

                    case Paradigm.FARTHEST:
                        return rawArr.GetMax(c => Vector3.SqrMagnitude(c.transform.position - transform.position));

                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }

        private void OnDrawGizmos()
        {
            if (debugRender)
            {
                Gizmos.DrawWireSphere(transform.position, radius);
            }
        }
    }
}
