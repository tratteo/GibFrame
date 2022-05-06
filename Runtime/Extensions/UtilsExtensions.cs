// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : UtilsExtensions.cs
//
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GibFrame.Extensions
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
            var index = -1;
            var r = UnityEngine.Random.Range(0F, 1F);
            while (r > 0)
            {
                r -= set.ElementAt(++index).ProvideSelectProbability();
            }
            return set.ElementAt(index);
        }

        public static T SelectWithProbability<T>(this List<T> set, System.Random random) where T : IProbSelectable
        {
            var index = -1;
            var r = (float)random.NextDouble();
            while (r > 0)
            {
                r -= set.ElementAt(++index).ProvideSelectProbability();
            }
            return set.ElementAt(index);
        }

        public static T SelectWithProbability<T>(this T[] set, Func<T, float> probabilityProvider)
        {
            return set.ToList().SelectWithProbability(probabilityProvider);
        }

        public static T SelectWithProbability<T>(this T[] set, System.Random random, Func<T, float> probabilityProvider)
        {
            return set.SelectWithProbability(random, probabilityProvider);
        }

        public static T SelectWithProbability<T>(this List<T> set, Func<T, float> probabilityProvider)
        {
            var index = -1;
            var r = UnityEngine.Random.Range(0F, 1F);
            while (r > 0)
            {
                r -= probabilityProvider(set.ElementAt(++index));
            }
            return set.ElementAt(index);
        }

        public static T SelectWithProbability<T>(this List<T> set, System.Random random, Func<T, float> probabilityProvider)
        {
            return set.ToArray().SelectWithProbability(random, probabilityProvider);
        }

        public static void NormalizeProbabilities<T>(this List<T> set, Func<T, float> value, Action<T, float> setProbability)
        {
            float sum = 0;
            foreach (var elem in set)
            {
                sum += value(elem);
            }
            if (sum == 0)
            {
                foreach (var elem1 in set)
                {
                    setProbability(elem1, 1F / set.Count);
                }
            }
            else
            {
                foreach (var elem1 in set)
                {
                    setProbability(elem1, value(elem1) / sum);
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

        public static void NormalizeProbabilities<T>(this T[] selectables, Func<T, float> getProbability, Action<T, float> setProbability)
        {
            NormalizeProbabilities(selectables.ToList(), getProbability, setProbability);
        }

        public static Color Redify(this Color color, float magnitude = 1F)
        {
            color.g -= magnitude;
            color.b -= magnitude;
            return color;
        }

        public static Color Greenify(this Color color, float magnitude = 1F)
        {
            color.r -= magnitude;
            color.b -= magnitude;
            return color;
        }

        public static Color Blueify(this Color color, float magnitude = 1F)
        {
            color.r -= magnitude;
            color.g -= magnitude;
            return color;
        }

        public static Color Blackify(this Color color, float magnitude = 1F)
        {
            color.r -= magnitude;
            color.g -= magnitude;
            color.b -= magnitude;
            return color;
        }

        public static Color Whiteify(this Color color, float magnitude = 1F)
        {
            color.r += magnitude;
            color.g += magnitude;
            color.b += magnitude;
            return color;
        }
    }
}