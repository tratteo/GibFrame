using System;
using UnityEngine;

namespace GibFrame
{
    public class ValueSystemBehaviour : MonoBehaviour, IValueSystem
    {
        [SerializeField] private float maxValue;

        public float Value => System.Value;

        public float MaxValue => maxValue;

        public float ValuePercentage => Value / maxValue;

        protected ValueSystem System { get; private set; }

        public event Action<float, float> Changed;

        protected virtual void OnEnable() => System.Changed += OnChanged;

        protected virtual void OnDisable() => System.Changed -= OnChanged;

        private void OnChanged(float before, float after) => Changed?.Invoke(before, after);

        private void Awake() => System = new ValueSystem(maxValue);
    }
}