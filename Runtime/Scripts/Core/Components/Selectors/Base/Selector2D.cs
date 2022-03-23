﻿// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : Selector.cs
//
// All Rights Reserved

using System;
using System.Collections.Generic;
using TypeReferences;
using UnityEngine;

namespace GibFrame
{
    public abstract class Selector : MonoBehaviour
    {
        [Header("Filters")]
        public LayerMask mask = ~0;
        [Header("Behaviour")]
        [Tooltip("Call the function for ISelectable components")]
        public bool notifySelectables = true;
        private readonly List<Predicate<Collider>> predicates = new List<Predicate<Collider>>();
        [SerializeField] private TargetType[] targetTypes;
        [Header("Debug")]
        [SerializeField] private bool debugRender = true;
        private Collider currentCollider;
        private ISelectable currentSelected = null;

        public bool Enabled { get; private set; } = true;

        public Collider Selected => currentCollider;

        protected bool DebugRender => debugRender;

        public event Action<Collider> OnSelected = delegate { };

        public event Action<Collider> OnDeselected = delegate { };

        public virtual void SetActive(bool state)
        {
            Enabled = state;
        }

        public void InjectPredicates(params Predicate<Collider>[] pred)
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
                OnDeselected?.Invoke(currentCollider);
            }
            if (currentSelected != null && notifySelectables)
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

        protected bool IsColliderValid(Collider collider)
        {
            foreach (var type in targetTypes)
            {
                if (collider.GetComponent(type.Type) is null)
                {
                    return false;
                }
            }
            foreach (var predicate in predicates)
            {
                if (predicate != null && !predicate(collider))
                {
                    return false;
                }
            }
            return collider.gameObject && !collider.gameObject.Equals(gameObject);
        }

        protected void Select(Collider newCollider)
        {
            if (newCollider && newCollider.Equals(currentCollider))
            {
                return;
            }
            ResetSelection();

            OnSelected?.Invoke(newCollider);
            currentCollider = newCollider;
            var newSelectable = newCollider.GetComponent<ISelectable>();
            currentSelected = newSelectable;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (targetTypes is null)
            {
                return;
            }

            foreach (var type in targetTypes)
            {
                if (type.Type is not null && !type.Type.IsInterface && !type.Type.IsSubclassOf(typeof(Component)))
                {
                    UnityEngine.Debug.LogError($"Selector[{gameObject}]: Target types must be either components or interfaces");
                }
            }
        }

#endif

        [Serializable]
        private struct TargetType
        {
            [SerializeField]
            private TypeReference type;

            public Type Type => type;
        }
    }
}
