// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : Selector.cs
//
// All Rights Reserved

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GibFrame.Selectors
{
    public abstract class Selector : MonoBehaviour
    {
        [Header("Filters")]
        public LayerMask mask = ~0;
        [Header("Behaviour")]
        private readonly List<Predicate<GameObject>> predicates = new List<Predicate<GameObject>>();
        [Header("Debug")]
        [SerializeField] private bool debugRender = true;
        private GameObject currentCollider;
        private ISelectable currentSelected = null;
        [Header("Advanced")]
        [SerializeField] private bool nonAlloc = false;
        [Tooltip("Define the maximum buffer size when using the non alloc option")]
        [SerializeField] private int bufferSize = 16;

        public bool Enabled { get; private set; } = true;

        public GameObject SelectedObj => currentCollider;

        protected int BufferSize => bufferSize;

        protected bool NonAlloc => nonAlloc;

        protected bool DebugRender => debugRender;

        public event Action<GameObject> Selected = delegate { };

        public event Action<GameObject> Deselected = delegate { };

        public virtual void SetActive(bool state)
        {
            Enabled = state;
        }

        public void InjectPredicates(params Predicate<GameObject>[] pred)
        {
            foreach (var p in pred)
            {
                predicates.Add(p);
            }
        }

        public void ClearPredicates()
        {
            predicates.Clear();
        }

        public void ResetSelection()
        {
            if (currentCollider)
            {
                Deselected?.Invoke(currentCollider);
            }
            if (currentSelected != null)
            {
                currentSelected.OnDeselect();
            }
            currentSelected = null;
            currentCollider = null;
        }

        protected virtual void Start()
        {
        }

        protected virtual void Awake()
        {
        }

        protected bool IsGameObjectValid(GameObject collider)
        {
            foreach (var predicate in predicates)
            {
                if (predicate != null && !predicate(collider))
                {
                    return false;
                }
            }
            return collider.gameObject && !collider.Equals(gameObject);
        }

        protected void Select(GameObject newCollider)
        {
            if (newCollider && newCollider.Equals(currentCollider))
            {
                return;
            }
            ResetSelection();

            Selected?.Invoke(newCollider);
            currentCollider = newCollider;
            var newSelectable = newCollider.GetComponent<ISelectable>();
            currentSelected = newSelectable;
        }
    }
}
