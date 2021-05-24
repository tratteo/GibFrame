// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : PathingOption.cs
//
// All Rights Reserved

using System;
using UnityEngine;

namespace GibFrame
{
    [System.Serializable]
    internal class PathingOption : IProbSelectable, IEquatable<PathingOption>
    {
        [SerializeField] private Waypoint option;
        [Range(0F, 1F)]
        [SerializeField] private float pickProb;

        public Waypoint Point { get => option; }

        public PathingOption(Waypoint option) : this(option, 1F)
        { }

        public PathingOption(Waypoint option, float prob)
        {
            this.option = option;
            this.pickProb = prob;
        }

        public float ProvideSelectProbability()
        {
            return pickProb;
        }

        public void SetSelectProbability(float prob)
        {
            pickProb = prob;
        }

        public bool Equals(PathingOption other)
        {
            return other.Point.Equals(this.Point);
        }
    }
}