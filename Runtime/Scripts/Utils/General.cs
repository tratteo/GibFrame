// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.Utils : General.cs
//
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Linq;

namespace GibFrame.Utils
{
    public class General
    {
        /// <summary>
        ///   Copy an array to another array ignoring the elements at indexes: <paramref name="indexesExcepts"/>
        /// </summary>
        public static void CopyArrayWithExceptsAt(Array from, Array to, int[] indexesExcepts)
        {
            Array.Sort(indexesExcepts);
            if (from.Length - to.Length != indexesExcepts.Length)
            {
                throw new System.Exception("Unable to copy arrays of wrong dimensions");
            }

            int exIndex = 0;
            int toIndex = 0;
            for (int i = 0; i < from.Length; i++)
            {
                if (i != indexesExcepts[exIndex])
                {
                    to.SetValue(from.GetValue(i), toIndex++);
                }
                else
                {
                    if (exIndex < indexesExcepts.Length - 1)
                        exIndex++;
                }
            }
        }

        public static T[] ShuffleArray<T>(T[] arr, int seed)
        {
            Random prng = new Random(seed);
            for (int i = 0; i < arr.Length - 1; i++)
            {
                int index = prng.Next(i, arr.Length);
                T temp = arr[index];
                arr[index] = arr[i];
                arr[i] = temp;
            }
            return arr;
        }

        /// <summary>
        ///   Copy an array to another array leaving holes at indexes: <paramref name="indexesExcepts"/>
        /// </summary>
        public static void CopyArrayWithHolesAt(Array from, Array to, int[] indexesExcepts)
        {
            Array.Sort(indexesExcepts);
            if (to.Length - from.Length != indexesExcepts.Length)
            {
                throw new System.Exception("Unable to copy arrays of wrong dimensions");
            }

            int elementToCopy = 0;
            int exIndex = 0;

            for (int i = 0; i < to.Length; i++)
            {
                if (exIndex > indexesExcepts.Length - 1)
                {
                    to.SetValue(from.GetValue(elementToCopy++), i);
                }
                else if (i != indexesExcepts[exIndex])
                {
                    to.SetValue(from.GetValue(elementToCopy++), i);
                }
                else
                {
                    exIndex++;
                }
            }
        }

        /// <summary>
        ///   For each element of the first list, zips the first predicate matching element from the second list
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <typeparam name="E"> </typeparam>
        /// <param name="first"> </param>
        /// <param name="second"> </param>
        /// <param name="predicate"> </param>
        /// <returns> The zipped list </returns>
        public static List<Tuple<T, E>> ZipWithFirstPredicateMatching<T, E>(List<T> first, List<E> second, Func<T, E, bool> predicate)
        {
            List<Tuple<T, E>> res = new List<Tuple<T, E>>();
            foreach (T elem in first)
            {
                foreach (E elem2 in second)
                {
                    if (predicate(elem, elem2))
                    {
                        res.Add(Tuple.Create(elem, elem2));
                        break;
                    }
                }
            }
            return res;
        }

        /// <summary>
        ///   Select an item based on its probability (interface)
        /// </summary>
        /// <param name="set"> </param>
        /// <returns> The selecte item </returns>
        public static T SelectWithProbability<T>(List<T> set) where T : IProbSelectable
        {
            int index = -1;
            float r = UnityEngine.Random.Range(0F, 1F);
            while (r > 0)
            {
                r -= set.ElementAt(++index).ProvideSelectProbability();
            }
            return (T)set.ElementAt(index);
        }

        public static T SelectWithProbability<T>(List<T> set, System.Random random) where T : IProbSelectable
        {
            int index = -1;
            float r = (float)random.NextDouble();
            while (r > 0)
            {
                r -= set.ElementAt(++index).ProvideSelectProbability();
            }
            return (T)set.ElementAt(index);
        }

        /// <summary>
        ///   Normalize the select probabilities using the <b> float value(T:IProbSelectable) </b> function
        /// </summary>
        /// <param name="set"> </param>
        /// <returns> </returns>
        public static void NormalizeProbabilities<T>(List<T> set, Func<T, float> value) where T : IProbSelectable
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
                    elem1.SetSelectProbability(1F / set.Count);
                }
            }
            else
            {
                foreach (T elem1 in set)
                {
                    elem1.SetSelectProbability(value(elem1) / sum);
                }
            }
        }

        public static void NormalizeProbabilities<T>(List<T> selectables) where T : IProbSelectable
        {
            NormalizeProbabilities(selectables, (s) => s.ProvideSelectProbability());
        }

        public static void NormalizeProbabilities<T>(T[] selectables) where T : IProbSelectable
        {
            NormalizeProbabilities(selectables.ToList());
        }

        public static T GetPredicateMaxObject<T>(T[] set, Func<T, double> predicate)
        {
            double value = double.MinValue;
            double p = 0;
            T current = default(T);
            for (int i = 0; i < set.Length; i++)
            {
                if ((p = predicate(set[i])) > value)
                {
                    value = p;
                    current = set[i];
                }
            }
            return current;
        }

        public static T GetPredicateMaxObject<T>(List<T> set, Func<T, double> predicate)
        {
            return GetPredicateMaxObject(set.ToArray(), predicate);
        }

        public static T GetPredicateMinObject<T>(T[] set, Func<T, double> predicate)
        {
            double value = double.MaxValue;
            double p = 0;
            T current = default(T);
            for (int i = 0; i < set.Length; i++)
            {
                if ((p = predicate(set[i])) < value)
                {
                    value = p;
                    current = set[i];
                }
            }
            return current;
        }

        public static List<T> GetPredicateMinObjects<T>(T[] set, Func<T, double> predicate)
        {
            T obj = GetPredicateMinObject(set, predicate);
            List<T> elems = new List<T>();
            foreach (T elem in set)
            {
                if (predicate(elem) == predicate(obj))
                {
                    elems.Add(elem);
                }
            }
            return elems;
        }

        public static List<T> GetPredicateMaxObjects<T>(T[] set, Func<T, double> predicate)
        {
            T obj = GetPredicateMaxObject(set, predicate);
            List<T> elems = new List<T>();
            foreach (T elem in set)
            {
                if (predicate(elem) == predicate(obj))
                {
                    elems.Add(elem);
                }
            }
            return elems;
        }

        public static T GetPredicateMinObject<T>(List<T> set, Func<T, double> predicate)
        {
            return GetPredicateMinObject(set.ToArray(), predicate);
        }

        public static List<T> GetPredicatesMatchingObjects<T>(T[] set, params Func<T, bool>[] predicates)
        {
            List<T> matching = new List<T>();
            foreach (T elem in set)
            {
                bool accept = true;
                foreach (Func<T, bool> current in predicates)
                {
                    if (!current(elem))
                    {
                        accept = false;
                        break;
                    }
                }
                if (accept)
                {
                    matching.Add(elem);
                }
            }
            return matching;
        }

        public static List<T> GetPredicatesMatchingObjects<T>(List<T> set, params Func<T, bool>[] predicates)
        {
            return GetPredicatesMatchingObjects(set.ToArray(), predicates);
        }

        public static List<T> ConvertAllTo<T, E>(List<E> source, Func<E, T> converter)
        {
            List<T> ret = new List<T>();
            foreach (E elem in source)
            {
                ret.Add(converter(elem));
            }
            return ret;
        }
    }
}