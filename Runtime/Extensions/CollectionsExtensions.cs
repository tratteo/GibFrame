// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : CollectionsExtensions.cs
//
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Linq;

namespace GibFrame.Extensions
{
    public static class CollectionsExtensions
    {
        #region Permutations

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> enumerable)
        {
            var array = enumerable as T[] ?? enumerable.ToArray();

            var factorials = Enumerable.Range(0, array.Length + 1)
                .Select(Factorial)
                .ToArray();

            for (var i = 0L; i < factorials[array.Length]; i++)
            {
                var sequence = GenerateSequence(i, array.Length - 1, factorials);

                yield return GeneratePermutation(array, sequence);
            }
        }

        private static IEnumerable<T> GeneratePermutation<T>(T[] array, IReadOnlyList<int> sequence)
        {
            var clone = (T[])array.Clone();

            for (var i = 0; i < clone.Length - 1; i++)
            {
                Swap(ref clone[i], ref clone[i + sequence[i]]);
            }

            return clone;
        }

        private static int[] GenerateSequence(long number, int size, IReadOnlyList<long> factorials)
        {
            var sequence = new int[size];

            for (var j = 0; j < sequence.Length; j++)
            {
                var facto = factorials[sequence.Length - j];

                sequence[j] = (int)(number / facto);
                number = (int)(number % facto);
            }

            return sequence;
        }

        private static void Swap<T>(ref T a, ref T b) => (b, a) = (a, b);

        private static long Factorial(int n)
        {
            long result = n;

            for (var i = 1; i < n; i++)
            {
                result = result * i;
            }

            return result;
        }

        #endregion Permutations

        public static T GetMax<T>(this IEnumerable<T> set, Func<T, double> predicate)
        {
            var value = double.MinValue;
            double p = 0;
            var current = default(T);
            for (var i = 0; i < set.Count(); i++)
            {
                if ((p = predicate(set.ElementAt(i))) > value)
                {
                    value = p;
                    current = set.ElementAt(i);
                }
            }
            return current;
        }

        public static T GetMin<T>(this IEnumerable<T> set, Func<T, double> predicate)
        {
            var value = double.MaxValue;
            var current = default(T);
            for (var i = 0; i < set.Count(); i++)
            {
                double p;
                if ((p = predicate(set.ElementAt(i))) < value)
                {
                    value = p;
                    current = set.ElementAt(i);
                }
            }
            return current;
        }

        public static List<T> GetMins<T>(this IEnumerable<T> set, Func<T, double> predicate)
        {
            var obj = GetMin(set, predicate);
            var elems = new List<T>();
            foreach (var elem in set)
            {
                if (predicate(elem) == predicate(obj))
                {
                    elems.Add(elem);
                }
            }
            return elems;
        }

        public static List<T> GetMaxes<T>(this IEnumerable<T> set, Func<T, double> Predicate)
        {
            var obj = GetMax(set, Predicate);
            var elems = new List<T>();
            foreach (var elem in set)
            {
                if (Predicate(elem) == Predicate(obj))
                {
                    elems.Add(elem);
                }
            }
            return elems;
        }

        /// <summary>
        ///   Reorders the array with the matching elements in the first positions up to the index specified by the return value
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="set"> </param>
        /// <param name="predicates"> </param>
        /// <returns> The number of matching elements </returns>
        public static int GetPredicatestMatchingObjectsNonAlloc<T>(this T[] set, params Predicate<T>[] predicates)
        {
            var res = 0;
            var index = 0;
            var valid = true;
            for (var i = 0; i < set.Count(); i++)
            {
                foreach (var predicate in predicates)
                {
                    if (!predicate(set.ElementAt(i)))
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    (set[i], set[index]) = (set[index], set[i]);
                    index++;
                    res++;
                }
                valid = true;
            }
            return res;
        }

        /// <summary>
        ///   Reorders the array with the matching elements in the first positions up to the index specified by the return value
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="set"> </param>
        /// <param name="predicates"> </param>
        /// <returns> The number of matching elements </returns>
        public static int GetPredicatestMatchingObjectsNonAlloc<T>(this List<T> set, params Predicate<T>[] predicates)
        {
            var res = 0;
            var index = 0;
            var valid = true;
            for (var i = 0; i < set.Count(); i++)
            {
                foreach (var predicate in predicates)
                {
                    if (!predicate(set.ElementAt(i)))
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    (set[i], set[index]) = (set[index], set[i]);
                    index++;
                    res++;
                }
                valid = true;
            }
            return res;
        }

        public static T PickRandom<T>(this IEnumerable<T> set, int seed)
        {
            return set.ElementAt(new Random(seed).Next(0, set.Count()));
        }

        public static List<T> Shuffle<T>(this List<T> set, int seed)
        {
            var prng = new Random(seed);
            for (var i = 0; i < set.Count - 1; i++)
            {
                var index = prng.Next(i, set.Count);
                (set[i], set[index]) = (set[index], set[i]);
            }
            return set;
        }

        public static T[] Shuffle<T>(this T[] arr, int seed)
        {
            var prng = new Random(seed);
            for (var i = 0; i < arr.Length - 1; i++)
            {
                var index = prng.Next(i, arr.Length);
                (arr[i], arr[index]) = (arr[index], arr[i]);
            }
            return arr;
        }
    }
}