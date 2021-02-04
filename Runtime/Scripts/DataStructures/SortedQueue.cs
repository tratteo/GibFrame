using System;
using System.Collections.Generic;
using System.Linq;

namespace GibFrame.DataStructures
{
    public enum SortingType { DESCENDING, ASCENDING }

    public class SortedQueue<T> where T : IComparable<T>
    {
        private List<T> objects;
        private SortingType sorting;

        public int Count { get => objects.Count; }

        public SortedQueue(SortingType sorting = SortingType.DESCENDING)
        {
            objects = new List<T>();
            this.sorting = sorting;
        }

        public void Enqueue(T val)
        {
            objects.Add(val);
            switch (sorting)
            {
                case SortingType.ASCENDING:
                    objects = objects.OrderBy((x) => x).ToList();
                    break;

                case SortingType.DESCENDING:
                    objects = objects.OrderByDescending((x) => x).ToList();
                    break;
            }
        }

        public T Dequeue()
        {
            if (objects.Count > 0)
            {
                T elem = objects.ElementAt(0);
                objects.RemoveAt(0);
                return elem;
            }
            else
            {
                return default;
            }
        }

        public T Last()
        {
            if (objects.Count > 0)
            {
                return objects.ElementAt(objects.Count - 1);
            }
            else
            {
                return default;
            }
        }

        public T First()
        {
            if (objects.Count > 0)
            {
                return objects.ElementAt(0);
            }
            else
            {
                return default;
            }
        }

        public void Clear() => objects.Clear();
    }
}