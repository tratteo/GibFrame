// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.Selectors : DistanceBasedSelector.cs
//
// All Rights Reserved

using System.Collections;
using UnityEngine;

namespace GibFrame.Selectors
{
    public class DistanceBasedSelector : DiscreteSelector
    {
        public enum Paradigm { CLOSEST, FARTHEST }

        [SerializeField] private Paradigm paradigm;
        [SerializeField] private float senseRadius = 8F;

        protected override IEnumerator SelectCoroutine()
        {
            yield return new WaitForSecondsRealtime(updateInterval);
            while (true)
            {
                if (Active)
                {
                    Collider selected = null;
                    Collider[] colliders = Physics.OverlapSphere(transform.position, senseRadius);
                    colliders = Utils.General.GetPredicatesMatchingObjects(colliders, (c) => c.CompareTag(selectableTag) && ColliderSatisfiesPredicates(c));
                    if (colliders != null && colliders.Length > 0)
                    {
                        switch (paradigm)
                        {
                            case Paradigm.CLOSEST:
                                selected = Utils.General.GetPredicateMinObject(colliders, c => Vector3.Distance(transform.position, c.transform.position));
                                break;

                            case Paradigm.FARTHEST:
                                selected = Utils.General.GetPredicateMaxObject(colliders, c => Vector3.Distance(transform.position, c.transform.position));
                                break;
                        }
                        Select(selected);
                    }
                    else
                    {
                        ResetSelection();
                    }
                }
                yield return new WaitForSeconds(updateInterval);
            }
        }
    }
}
