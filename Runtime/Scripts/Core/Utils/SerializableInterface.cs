using System;
using UnityEngine;

namespace GibFrame
{
    [Serializable]
    public class SerializableInterface<T> where T : class
    {
        [SerializeField, Readonly] private string interfaceType = typeof(T).AssemblyQualifiedName;
        [SerializeField] private Behaviour interfaceBehaviour;

        public T Value => this;

        public static implicit operator T(SerializableInterface<T> interfaceGeneric) => interfaceGeneric.interfaceBehaviour as T;
    }
}
