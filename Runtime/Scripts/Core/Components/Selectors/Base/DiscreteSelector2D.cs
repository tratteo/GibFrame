// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : DiscreteSelector.cs
//
// All Rights Reserved

using System.Collections;
using UnityEngine;

namespace GibFrame
{
    public abstract class DiscreteSelector2D : Selector2D
    {
        [Header("Discrete selector")]
        [SerializeField] protected float updateInterval = 0.2F;
        [SerializeField] private bool enabledOnStart = true;
        private Coroutine coroutine;

        public override void SetActive(bool state)
        {
            base.SetActive(state);
            if (state)
            {
                if (coroutine is null)
                {
                    coroutine = StartCoroutine(SelectCoroutine());
                }
            }
            else if (coroutine is not null)
            {
                StopCoroutine(coroutine);
            }
        }

        protected abstract IEnumerator SelectCoroutine();

        protected override void Start()
        {
            if (enabledOnStart)
            {
                SetActive(true);
            }
        }
    }
}
