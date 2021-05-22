//Copyright (c) matteo
//Selector.cs - com.tratteo.gibframe

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GibFrame
{
    public abstract class Selector : MonoBehaviour
    {
        private readonly List<Predicate<Collider>> predicates = new List<Predicate<Collider>>();
        [SerializeField] private LayerMask mask = ~0;
        [SerializeField] private string selectableTag;
        [Tooltip("Call the function for ISelectable components")]
        [SerializeField] private bool callInterfaceFunctions = true;
        [Tooltip("Select only game objects that have a component that implements the ISelectable interface")]
        [SerializeField] private bool selectOnlyInterfaces = false;
        private Collider currentCollider;
        private ISelectable currentSelected = null;

        public int Mask { get => mask; set => mask = value; }

        public string SelectableTag { get => selectableTag; set => selectableTag = value; }

        public bool CallInterfaceFunctions { get => callInterfaceFunctions; set => callInterfaceFunctions = value; }

        public bool Active { get; private set; } = true;

        public bool SelectOnlyInterfaces { get => selectOnlyInterfaces; set => selectOnlyInterfaces = value; }

        public Collider ColliderSelection => currentCollider;

        public ISelectable SelectableSelection => currentSelected;

        public event Action<Collider> OnRawSelected = delegate { };

        public event Action<Collider> OnRawDeselected = delegate { };

        public event Action<ISelectable> OnDeselected = delegate { };

        public event Action<ISelectable> OnSelected = delegate { };

        public void InjectPredicates(params Predicate<Collider>[] pred)
        {
            foreach (Predicate<Collider> p in pred)
            {
                predicates.Add(p);
            }
        }

        public void ResetSelection()
        {
            if (currentCollider)
            {
                OnRawDeselected?.Invoke(currentCollider);
                if (currentSelected != null && callInterfaceFunctions)
                {
                    OnDeselected?.Invoke(currentSelected);
                    currentSelected.OnDeselect();
                }
            }

            currentSelected = null;
            currentCollider = null;
        }

        public void SetActive(bool state)
        {
            Active = state;
        }

        protected virtual void Start()
        {
        }

        protected virtual void Awake()
        {
        }

        protected bool IsColliderValid(Collider collider)
        {
            return ColliderSatisfiesPredicates(collider)
                && (selectableTag.Equals(string.Empty) || collider.CompareTag(selectableTag))
                && (!selectOnlyInterfaces || collider.GetComponent<ISelectable>() != null)
                && collider.gameObject && !collider.gameObject.Equals(gameObject);
        }

        protected void Select(Collider newCollider)
        {
            if (newCollider != null && !newCollider.Equals(currentCollider))
            {
                if (currentCollider)
                {
                    OnRawDeselected?.Invoke(currentCollider);
                }
                OnRawSelected?.Invoke(newCollider);
                currentCollider = newCollider;
                ISelectable newSelectable = newCollider.gameObject.GetComponent<ISelectable>();
                if (newSelectable != null && !newSelectable.Equals(currentSelected))
                {
                    if (currentSelected != null && callInterfaceFunctions && currentCollider.gameObject)
                    {
                        currentSelected.OnDeselect();
                        OnDeselected?.Invoke(currentSelected);
                    }

                    currentSelected = newSelectable;
                    if (callInterfaceFunctions)
                    {
                        currentSelected.OnSelect();
                        OnSelected?.Invoke(currentSelected);
                    }
                }
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