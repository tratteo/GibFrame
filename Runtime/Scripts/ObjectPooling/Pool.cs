// Copyright (c) 2020 Matteo Beltrame

using UnityEngine;

namespace GibFrame.ObjectPooling
{
    [System.Serializable]
    internal class Pool
    {
        public string tag;
        public GameObject prefab;
        public int poolSize;
        [Tooltip("The probability will be normalized based on other pools probability")]
        [Range(0f, 1f)]
        public float spawnProbability;
    }
}