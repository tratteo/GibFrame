// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : DiscreteSelector.cs
//
// All Rights Reserved

using System.Collections;
using UnityEngine;

namespace GibFrame
{
    public abstract class DiscreteSelector : Selector
    {
        [SerializeField] protected float updateInterval = 0.2F;

        protected abstract IEnumerator SelectCoroutine();

        protected override void Start()
        {
            StartCoroutine(SelectCoroutine());
        }
    }
}