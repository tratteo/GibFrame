// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : ArrayExtensions.cs
//
// All Rights Reserved

using System;
using System.Collections.Generic;

namespace GibFrame
{
    public static class ArrayExtensions
    {
        /// <summary>
        ///   Copy an array to another array ignoring the elements at indexes: <paramref name="indexesExcepts"/>
        /// </summary>
        public static void CopyArrayWithExceptsAt(this Array from, Array to, int[] indexesExcepts)
        {
            Array.Sort(indexesExcepts);
            if (from.Length - to.Length != indexesExcepts.Length)
            {
                throw new System.Exception("Unable to copy arrays of wrong dimensions");
            }

            var exIndex = 0;
            var toIndex = 0;
            for (var i = 0; i < from.Length; i++)
            {
                if (i != indexesExcepts[exIndex])
                {
                    to.SetValue(from.GetValue(i), toIndex++);
                }
                else
                {
                    if (exIndex < indexesExcepts.Length - 1)
                    {
                        exIndex++;
                    }
                }
            }
        }

        public static List<T> FindAll<T>(this T[] collection, int upTo, Predicate<T> predicate)
        {
            var match = new List<T>();
            for (var i = 0; i < collection.Length; i++)
            {
                if (i > upTo)
                {
                    return match;
                }

                var item = collection[i];
                if (predicate(item))
                {
                    match.Add(item);
                }
            }
            return match;
        }

        public static List<T> FindAll<T>(this T[] collection, Predicate<T> predicate)
        {
            var match = new List<T>();
            foreach (var item in collection)
            {
                if (predicate(item))
                {
                    match.Add(item);
                }
            }
            return match;
        }

        /// <summary>
        ///   Copy an array to another array leaving holes at indexes: <paramref name="indexesExcepts"/>
        /// </summary>
        public static void CopyArrayWithHolesAt(this Array from, Array to, int[] indexesExcepts)
        {
            Array.Sort(indexesExcepts);
            if (to.Length - from.Length != indexesExcepts.Length)
            {
                throw new System.Exception("Unable to copy arrays of wrong dimensions");
            }

            var elementToCopy = 0;
            var exIndex = 0;

            for (var i = 0; i < to.Length; i++)
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
    }
}
