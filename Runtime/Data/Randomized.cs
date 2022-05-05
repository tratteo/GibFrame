// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : Randomized.cs
//
// All Rights Reserved

using System;
using UnityEngine;

namespace GibFrame.Data
{
    public class Randomized<T>
    {
        private Func<T> Provider;

        public Randomized(Func<T> provider)
        {
            Provider = provider;
        }

        public static implicit operator T(Randomized<T> randomized) => randomized.GetNext();

        public T GetNext() => Provider();
    }

    [Serializable]
    public class RandomizedFloat
    {
        [SerializeField] private float min, max;
        private System.Random random;
        private Func<float> Provider = null;

        public float Min { get => min; set => min = value; }

        public float Max { get => max; set => max = value; }

        public RandomizedFloat(Func<float> provider)
        {
            Provider = provider;
        }

        public RandomizedFloat(float min = 0F, float max = 1F)
        {
            this.min = min;
            this.max = max;
            Provider = () => UnityEngine.Random.Range(min, max);
        }

        public RandomizedFloat(float min, float max, int seed)
        {
            this.min = min;
            this.max = max;
            random = new System.Random(seed);
            Provider = () => GibMath.Map((float)random.NextDouble(), (0F, 1F), (min, max));
        }

        public static implicit operator float(RandomizedFloat randomized) => randomized.GetNext();

        public override string ToString()
        {
            return "Min: " + min + ", Max: " + max;
        }

        public float GetNext()
        {
            Provider ??= () => UnityEngine.Random.Range(min, max);
            return Provider();
        }
    }

    [Serializable]
    public class RandomizedInt
    {
        [SerializeField] private int min, max;
        private System.Random random;
        private Func<int> Provider = null;

        public int Min { get => min; set => min = value; }

        public int Max { get => max; set => max = value; }

        public RandomizedInt(Func<int> provider)
        {
            Provider = provider;
        }

        public RandomizedInt(int min = 0, int max = 1)
        {
            this.min = min;
            this.max = max;
            Provider = () => UnityEngine.Random.Range(min, max);
        }

        public RandomizedInt(int min, int max, int seed)
        {
            this.min = min;
            this.max = max;
            random = new System.Random(seed);
            Provider = () => (int)GibMath.Map((float)random.NextDouble(), (0F, 1F), (min, max));
        }

        public static implicit operator int(RandomizedInt randomized) => randomized.GetNext();

        public override string ToString()
        {
            return "Min: " + min + ", Max: " + max;
        }

        public int GetNext()
        {
            Provider ??= () => UnityEngine.Random.Range(min, max);
            return Provider();
        }
    }
}
