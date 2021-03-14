//Copyright (c) matteo
//DistanceBasedSelector.cs - com.tratteo.gibframe

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
            Collider[] colliders;
            WaitForSeconds Delay = new WaitForSeconds(updateInterval);
            yield return Delay;
            while (true)
            {
                if (Active)
                {
                    Collider selected = null;
                    colliders = Physics.OverlapSphere(transform.position, senseRadius);
                    colliders = Utils.General.GetPredicatesMatchingObjects(colliders, (c) => c.CompareTag(selectableTag) && ColliderSatisfiesPredicates(c));
                    if (colliders != null && colliders.Length > 0)
                    {
                        switch (paradigm)
                        {
                            case Paradigm.CLOSEST:
                                selected = Utils.General.GetPredicateMinObject(colliders, c => Vector3.SqrMagnitude(c.transform.position - transform.position));
                                break;

                            case Paradigm.FARTHEST:
                                selected = Utils.General.GetPredicateMaxObject(colliders, c => Vector3.SqrMagnitude(c.transform.position - transform.position));
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