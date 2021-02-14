// Copyright (c) 2020 Matteo Beltrame

using System;
using System.Collections.Generic;
using UnityEngine;

namespace GibFrame.Selectors
{
    public abstract class Selector : MonoBehaviour
    {
        [SerializeField] protected string selectableTag;
        protected ISelectable currentSelected = null;
        protected Collider currentCollider;

        private List<Func<Collider, bool>> predicates = new List<Func<Collider, bool>>();

        public GameObject CurrentSelected { get => currentCollider != null ? currentCollider.gameObject : null; }

        public bool Active { get; private set; } = true;

        public T CurrentAs<T>() where T : class => currentSelected as T;

        public void InjectPredicates(params Func<Collider, bool>[] pred)
        {
            foreach (Func<Collider, bool> p in pred)
            {
                predicates.Add(p);
            }
        }

        public void SetActive(bool state)
        {
            Active = state;
        }

        public void ResetSelection()
        {
            currentSelected?.OnDeselect();
            currentSelected = null;
            currentCollider = null;
        }

        protected void Select(Collider newCollider)
        {
            if (newCollider != null)
            {
                currentCollider = newCollider;
                ISelectable newSelectable = currentCollider.gameObject.GetComponent<ISelectable>();
                if (newSelectable != null && currentSelected != newSelectable)
                {
                    if (currentSelected != null)
                    {
                        currentSelected?.OnDeselect();
                    }
                    currentSelected = newSelectable;
                    currentSelected.OnSelect();
                }
            }
        }

        protected bool ColliderSatisfiesPredicates(Collider collider)
        {
            foreach (Func<Collider, bool> func in predicates)
            {
                if (!func(collider))
                {
                    return false;
                }
            }
            return true;
        }
    }
}