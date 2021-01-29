// Copyright (c) 2020 Matteo Beltrame

using System.Collections;
using System.Collections.Generic;
using GibFrame.Utils;
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
                    List<Collider> selectables = Utils.General.GetPredicatesMatchingObjects(colliders, (c) => c.CompareTag(selectableTag));
                    if (selectables != null && selectables.Count > 0)
                    {
                        switch (paradigm)
                        {
                            case Paradigm.CLOSEST:
                                selected = Utils.General.GetPredicateMinObject(selectables, c => Vector3.Distance(transform.position, c.transform.position));
                                break;

                            case Paradigm.FARTHEST:
                                selected = Utils.General.GetPredicateMaxObject(selectables, c => Vector3.Distance(transform.position, c.transform.position));
                                break;
                        }
                        if (ColliderSatisfiesPredicates(selected))
                        {
                            Select(selected);
                        }
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