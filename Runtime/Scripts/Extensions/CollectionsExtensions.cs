using System;
using System.Collections.Generic;
using System.Linq;
using GibFrame.Utils;

namespace GibFrame.Extensions
{
    public static class CollectionsExtensions
    {
        public static T GetPredicateMaxObject<T>(this T[] set, Func<T, double> predicate)
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

        public static T GetPredicateMaxObject<T>(this List<T> set, Func<T, double> predicate)
        {
            return GetPredicateMaxObject(set.ToArray(), predicate);
        }

        public static T GetPredicateMinObject<T>(this T[] set, Func<T, double> predicate)
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

        public static List<T> GetPredicateMinObjects<T>(this T[] set, Func<T, double> predicate)
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

        public static List<T> GetPredicateMaxObjects<T>(this T[] set, Func<T, double> Predicate)
        {
            T obj = GetPredicateMaxObject(set, Predicate);
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

        public static T GetPredicateMinObject<T>(this List<T> set, Func<T, double> predicate)
        {
            return GetPredicateMinObject(set.ToArray(), predicate);
        }

        public static T[] GetPredicatesMatchingObjects<T>(this T[] set, params Predicate<T>[] predicates)
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

        public static List<T> GetPredicatesMatchingObjects<T>(this List<T> set, params Predicate<T>[] predicates)
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
            return matching;
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

        /// <summary>
        ///   For each element of the first list, zips the first predicate matching element from the second list
        /// </summary>
        /// <typeparam name="T1"> </typeparam>
        /// <typeparam name="T2"> </typeparam>
        /// <param name="first"> </param>
        /// <param name="second"> </param>
        /// <param name="predicate"> </param>
        /// <returns> The zipped list </returns>
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
