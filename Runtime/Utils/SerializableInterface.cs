using System;
using UnityEngine;

namespace GibFrame
{
    [Serializable]
    public class SerializableInterface<T> where T : class
    {
        [SerializeField] private UnityEngine.Object interfaceBehaviour;

        public T Value => this;

        public static implicit operator T(SerializableInterface<T> interfaceGeneric) => interfaceGeneric.interfaceBehaviour as T;
    }
}