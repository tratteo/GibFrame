//Copyright (c) matteo
//Selector.cs - com.tratteo.gibframe

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GibFrame.Selectors
{
    public abstract class Selector : MonoBehaviour
    {
        [SerializeField] protected LayerMask mask = ~0;
        [SerializeField] protected string selectableTag;
        [SerializeField] protected bool delegateActionOnInterface = true;
        private readonly List<Predicate<Collider>> predicates = new List<Predicate<Collider>>();
        private Collider currentCollider;
        private ISelectable currentSelected = null;

        public bool Active { get; private set; } = true;

        public GameObject CurrentSelected { get => currentCollider != null ? currentCollider.gameObject : null; }

        public event Action<ISelectable> OnDeselected;

        public event Action<ISelectable> OnSelected;

        public T CurrentAs<T>() where T : class => currentSelected as T;

        public void InjectPredicates(params Predicate<Collider>[] pred)
        {
            foreach (Predicate<Collider> p in pred)
            {
                predicates.Add(p);
            }
        }

        public void ResetSelection()
        {
            FireDeselect();
            currentSelected = null;
            currentCollider = null;
        }

        public void SetActive(bool state)
        {
            Active = state;
        }

        protected bool IsColliderValid(Collider collider)
        {
            return ColliderSatisfiesPredicates(collider) && (selectableTag.Equals(string.Empty) || collider.CompareTag(selectableTag)) && collider.GetComponent<ISelectable>() != null;
        }

        protected void Select(Collider newCollider)
        {
            if (newCollider != null)
            {
                ISelectable newSelectable = newCollider.gameObject.GetComponent<ISelectable>();
                if (newSelectable != null && currentSelected != newSelectable)
                {
                    currentCollider = newCollider;
                    FireDeselect();
                    currentSelected = newSelectable;
                    if (delegateActionOnInterface)
                    {
                        currentSelected.OnSelect();
                    }
                    OnSelected?.Invoke(currentSelected);
                }
            }
        }

        private void FireDeselect()
        {
            if (currentSelected != null)
            {
                if (currentCollider != null && delegateActionOnInterface && currentCollider.gameObject != null)
                {
                    currentSelected.OnDeselect();
                }
                OnDeselected?.Invoke(currentSelected);
            }
        }

        private bool ColliderSatisfiesPredicates(Collider collider)
        {
            foreach (Predicate<Collider> Predicate in predicates)
            {
                if (Predicate != null && !Predicate(collider))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
