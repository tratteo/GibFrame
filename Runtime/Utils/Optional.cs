using System;
using UnityEngine;

namespace GibFrame
{
    /// <summary>
    ///   Define a wrapper for an object that can safely be null
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    [Serializable]
    public class Optional<T> where T : class
    {
        [SerializeField] private T obj;

        public Optional(Func<T> factory)
        {
            obj = factory();
        }

        public Optional()
        {
            obj = null;
        }

        /// <summary>
        ///   Try to retrieve a value from the underlying object
        /// </summary>
        /// <typeparam name="TResult"> </typeparam>
        /// <param name="provider"> </param>
        /// <returns> The value if <see cref="obj"/> is not null, <see cref="default"/> otherwise </returns>
        public bool TryGet<TResult>(Func<T, TResult> provider, out TResult result)
        {
            if (IsNull())
            {
                result = default;
                return false;
            }
            result = provider(obj);
            return true;
        }

        /// <summary>
        ///   Try to retrieve the underlying optional
        /// </summary>
        /// <typeparam name="TResult"> </typeparam>
        /// <param name="provider"> </param>
        /// <returns> The value if <see cref="obj"/> is not null, <see cref="default"/> otherwise </returns>
        public bool TryGet(out T result) => TryGet((elem) => elem, out result);

        /// <summary>
        ///   Try execute a method on the underlying object
        /// </summary>
        /// <param name="func"> </param>
        public void Try(Action<T> func)
        {
            if (IsNull())
            {
                return;
            }

            func?.Invoke(obj);
        }

        public bool IsNull()
        {
            if (obj is IOptional optional && !optional.HasValue()) return true;
            if (obj is UnityEngine.Object uObj && !uObj) return true;
            if (obj is null) return true;
            return false;
        }
    }
}