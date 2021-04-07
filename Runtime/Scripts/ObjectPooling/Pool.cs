//Copyright (c) matteo
//Pool.cs - com.tratteo.gibframe

using System;
using GibFrame.Utils;
using UnityEngine;

namespace GibFrame.ObjectPooling
{
    [Serializable]
    public class Pool : IProbSelectable, IEquatable<Pool>
    {
        [SerializeField] private string tag;
        [SerializeField] private GameObject prefab;
        [SerializeField] private int size;
        [Tooltip("The probability will be normalized based on other pools probability")]
        [Range(0F, 1F)]
        [SerializeField] private float influence;

        public string Tag => tag;

        public GameObject Prefab => prefab;

        public int Size => size;

        public float Influence => influence;

        public Pool(string tag, GameObject prefab, int size, float influence = 1F)
        {
            this.tag = tag;
            this.prefab = prefab;
            this.size = size;
            this.influence = influence;
        }

        public float ProvideSelectProbability() => influence;

        public void SetSelectProbability(float prob)
        {
            influence = prob;
        }

        public bool Equals(Pool other)
        {
            return other.Tag.Equals(Tag);
        }
    }
}
