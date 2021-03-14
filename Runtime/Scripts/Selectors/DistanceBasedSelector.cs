//Copyright (c) matteo
//DistanceBasedSelector.cs - com.tratteo.gibframe

using System.Collections;
using GibFrame.Extensions;
using GibFrame.Physic;
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
            Collider[] colliders;
            WaitForSeconds Delay = new WaitForSeconds(updateInterval);
            yield return Delay;
            while (true)
            {
                if (Active)
                {
                    Collider selected = null;
                    colliders = GPhysics.MatchingOverlapSphere(transform.position, senseRadius, (c) => c.CompareTag(selectableTag) && ColliderSatisfiesPredicates(c));
                    if (colliders != null && colliders.Length > 0)
                    {
                        switch (paradigm)
                        {
                            case Paradigm.CLOSEST:
                                selected = colliders.GetPredicateMinObject(c => Vector3.SqrMagnitude(c.transform.position - transform.position));
                                break;

                            case Paradigm.FARTHEST:
                                selected = colliders.GetPredicateMaxObject(c => Vector3.SqrMagnitude(c.transform.position - transform.position));
                                break;
                        }
                        Select(selected);
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
