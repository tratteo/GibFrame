//Copyright (c) matteo
//CollectionsExtensions.cs - com.tratteo.gibframe

using System;
using System.Collections.Generic;
using System.Linq;

namespace GibFrame
{
    public static class CollectionsExtensions
    {
        public static void ForEach<T>(this T[] set, Action<T> Function)
        {
            foreach (T elem in set)
            {
                Function(elem);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> set, int end, Action<T> Function)
        {
            set.ForEach(0, end, Function);
        }

        public static void ForEach<T>(this IEnumerable<T> set, int start, int end, Action<T> Function)
        {
            for (int i = start; i < end; i++)
            {
                Function(set.ElementAt(i));
            }
        }

        public static TOutput[] ConvertAll<TInput, TOutput>(this TInput[] set, Converter<TInput, TOutput> Converter)
        {
            TOutput[] res = new TOutput[set.Length];
            for (int i = 0; i < set.Length; i++)
            {
                res[i] = Converter(set[i]);
            }
            return res;
        }

        public static T GetMax<T>(this IEnumerable<T> set, Func<T, double> predicate)
        {
            double value = double.MinValue;
            double p = 0;
            T current = default(T);
            for (int i = 0; i < set.Count(); i++)
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
            double value = double.MaxValue;
            double p = 0;
            T current = default(T);
            for (int i = 0; i < set.Count(); i++)
            {
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
            T obj = GetMin(set, predicate);
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

        public static List<T> GetMaxs<T>(this IEnumerable<T> set, Func<T, double> Predicate)
        {
            T obj = GetMax(set, Predicate);
            List<T> elems = new List<T>();
            foreach (T elem in set)
            {
                if (Predicate(elem) == Predicate(obj))
                {
                    elems.Add(elem);
                }
            }
            return elems;
        }

        public static T[] GetPredicatesMatchingObjects<T>(this IEnumerable<T> set, params Predicate<T>[] predicates)
        {
            List<T> matching = new List<T>();
            foreach (T elem in set)
            {
                bool accept = true;
                foreach (Predicate<T> current in predicates)
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
            return matching.ToArray();
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
            int res = 0;

            int index = 0;
            bool valid = true;
            for (int i = 0; i < set.Count(); i++)
            {
                foreach (Predicate<T> predicate in predicates)
                {
                    if (!predicate(set.ElementAt(i)))
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    T temp = set[index];
                    set[index] = set[i];
                    set[i] = temp;
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
            int res = 0;
            int index = 0;
            bool valid = true;
            for (int i = 0; i < set.Count(); i++)
            {
                foreach (Predicate<T> predicate in predicates)
                {
                    if (!predicate(set.ElementAt(i)))
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    T temp = set[index];
                    set[index] = set[i];
                    set[i] = temp;
                    index++;
                    res++;
                }
                valid = true;
            }
            return res;
        }

        public static int ForEachMatching<T>(this IEnumerable<T> set, Predicate<T> Predicate, Action<T> Operation)
        {
            int count = 0;
            foreach (T elem in set)
            {
                if (Predicate(elem))
                {
                    Operation(elem);
                    count++;
                }
            }
            return count;
        }

        public static T PickRandom<T>(this IEnumerable<T> set, int seed)
        {
            return set.ElementAt(new Random(seed).Next(0, set.Count()));
        }

        public static List<T> Shuffle<T>(this List<T> set, int seed)
        {
            Random prng = new Random(seed);
            for (int i = 0; i < set.Count - 1; i++)
            {
                int index = prng.Next(i, set.Count);
                T temp = set[index];
                set[index] = set[i];
                set[i] = temp;
            }
            return set;
        }

        public static T[] Shuffle<T>(this T[] arr, int seed)
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

        public static List<Tuple<T1, T2>> ZipWithFirstPredicateMatching<T1, T2>(this List<T1> first, List<T2> second, Func<T1, T2, bool> predicate)
        {
            List<Tuple<T1, T2>> res = new List<Tuple<T1, T2>>();
            foreach (T1 elem in first)
            {
                foreach (T2 elem2 in second)
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
    }
}