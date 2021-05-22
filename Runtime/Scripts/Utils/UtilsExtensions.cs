//Copyright (c) matteo
//UtilsExtensions.cs - com.tratteo.gibframe

using System;
using System.Collections.Generic;
using System.Linq;

namespace GibFrame
{
    public static class UtilsExtensions
    {
        public static T SelectWithProbability<T>(this T[] set) where T : IProbSelectable
        {
            return set.ToList().SelectWithProbability();
        }

        public static T SelectWithProbability<T>(this T[] set, System.Random random) where T : IProbSelectable
        {
            return set.ToList().SelectWithProbability(random);
        }

        public static T SelectWithProbability<T>(this List<T> set) where T : IProbSelectable
        {
            int index = -1;
            float r = UnityEngine.Random.Range(0F, 1F);
            while (r > 0)
            {
                r -= set.ElementAt(++index).ProvideSelectProbability();
            }
            return (T)set.ElementAt(index);
        }

        public static T SelectWithProbability<T>(this List<T> set, System.Random random) where T : IProbSelectable
        {
            int index = -1;
            float r = (float)random.NextDouble();
            while (r > 0)
            {
                r -= set.ElementAt(++index).ProvideSelectProbability();
            }
            return (T)set.ElementAt(index);
        }

        public static T SelectWithProbability<T>(this T[] set, Func<T, float> ProbabilityProvider)
        {
            return set.ToList().SelectWithProbability(ProbabilityProvider);
        }

        public static T SelectWithProbability<T>(this T[] set, System.Random random, Func<T, float> ProbabilityProvider)
        {
            return set.SelectWithProbability(random, ProbabilityProvider);
        }

        public static T SelectWithProbability<T>(this List<T> set, Func<T, float> ProbabilityProvider)
        {
            int index = -1;
            float r = UnityEngine.Random.Range(0F, 1F);
            while (r > 0)
            {
                r -= ProbabilityProvider(set.ElementAt(++index));
            }
            return (T)set.ElementAt(index);
        }

        public static T SelectWithProbability<T>(this List<T> set, System.Random random, Func<T, float> ProbabilityProvider)
        {
            return set.ToArray().SelectWithProbability(random, ProbabilityProvider);
        }

        public static void NormalizeProbabilities<T>(this List<T> set, Func<T, float> value, Action<T, float> SetProbability)
        {
            float sum = 0;
            foreach (T elem in set)
            {
                sum += value(elem);
            }
            if (sum == 0)
            {
                foreach (T elem1 in set)
                {
                    SetProbability(elem1, 1F / set.Count);
                }
            }
            else
            {
                foreach (T elem1 in set)
                {
                    SetProbability(elem1, value(elem1) / sum);
                }
            }
        }

        public static void NormalizeProbabilities<T>(this List<T> selectables) where T : IProbSelectable
        {
            NormalizeProbabilities(selectables, (s) => s.ProvideSelectProbability(), (s, p) => s.SetSelectProbability(p));
        }

        public static void NormalizeProbabilities<T>(this T[] selectables) where T : IProbSelectable
        {
            NormalizeProbabilities(selectables.ToList());
        }

        public static void NormalizeProbabilities<T>(this T[] selectables, Func<T, float> GetProbability, Action<T, float> SetProbability)
        {
            NormalizeProbabilities(selectables.ToList(), GetProbability, SetProbability);
        }
    }
}