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
    public abstract class Selector2D : MonoBehaviour
    {
        [Header("Filters")]
        public LayerMask mask = ~0;
        [Header("Behaviour")]
        [Tooltip("Call the function for ISelectable components")]
        public bool notifySelectables = true;
        private readonly List<Predicate<Collider2D>> predicates = new List<Predicate<Collider2D>>();
        [Header("Debug")]
        [SerializeField] private bool debugRender = true;
        private Collider2D currentCollider2D;
        private ISelectable currentSelected = null;

        public virtual bool Enabled { get; private set; } = true;

        public Collider2D Selected => currentCollider2D;

        protected bool DebugRender => debugRender;

        public event Action<Collider2D> OnSelected = delegate { };

        public event Action<Collider2D> OnDeselected = delegate { };

        public virtual void SetActive(bool state)
        {
            Enabled = state;
        }

        public void InjectPredicates(params Predicate<Collider2D>[] pred)
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
            if (currentCollider2D)
            {
                OnDeselected?.Invoke(currentCollider2D);
            }
            if (currentSelected != null && notifySelectables)
            {
                currentSelected.OnDeselect();
            }
            currentSelected = null;
            currentCollider2D = null;
        }

        protected virtual void Start()
        {
        }

        protected virtual void Awake()
        {
        }

        protected bool IsCollider2DValid(Collider2D Collider2D)
        {
            foreach (var predicate in predicates)
            {
                if (predicate != null && !predicate(Collider2D))
                {
                    return false;
                }
            }
            return Collider2D.gameObject && !Collider2D.gameObject.Equals(gameObject);
        }

        protected void Select(Collider2D newCollider2D)
        {
            if (newCollider2D && newCollider2D.Equals(currentCollider2D))
            {
                return;
            }
            ResetSelection();

            OnSelected?.Invoke(newCollider2D);
            currentCollider2D = newCollider2D;
            var newSelectable = newCollider2D.GetComponent<ISelectable>();
            currentSelected = newSelectable;
        }
    }
}
