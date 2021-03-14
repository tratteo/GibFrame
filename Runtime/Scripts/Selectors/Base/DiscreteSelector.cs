//Copyright (c) matteo
//DiscreteSelector.cs - com.tratteo.gibframe

using System.Collections;
using UnityEngine;

namespace GibFrame.Selectors
{
    public abstract class DiscreteSelector : Selector
    {
        [SerializeField] protected float updateInterval = 0.2F;

        protected abstract IEnumerator SelectCoroutine();

        private void Start()
        {
            StartCoroutine(SelectCoroutine());
        }
    }
}