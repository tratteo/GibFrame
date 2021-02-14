// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.Selectors : DiscreteSelector.cs
//
// All Rights Reserved

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