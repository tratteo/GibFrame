using System;
using GibFrame.Utils.Mathematics;

namespace GibFrame.DataStructures
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
        public float min, max;
        private Random random;
        private Func<float> Provider = null;

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
            random = new Random(seed);
            Provider = () => GMath.Map((float)random.NextDouble(), (0F, 1F), (min, max));
        }

        public static implicit operator float(RandomizedFloat randomized) => randomized.GetNext();

        public float GetNext()
        {
            Provider ??= () => UnityEngine.Random.Range(min, max);
            return Provider();
        }
    }

    [Serializable]
    public class RandomizedInt
    {
        public int min, max;
        private Random random;
        private Func<int> Provider = null;

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
            random = new Random(seed);
            Provider = () => (int)GMath.Map((float)random.NextDouble(), (0F, 1F), (min, max));
        }

        public static implicit operator int(RandomizedInt randomized) => randomized.GetNext();

        public int GetNext()
        {
            Provider ??= () => UnityEngine.Random.Range(min, max);
            return Provider();
        }
    }
}
