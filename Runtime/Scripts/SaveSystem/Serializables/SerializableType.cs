// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.SaveSystem.Serializables : SerializableType.cs
//
// All Rights Reserved

using System;

namespace GibFrame.SaveSystem.Serializables
{
    [System.Serializable]
    public class SerializableType : IEquatable<SerializableType>
    {
        private string name;
        private string assemblyQualifiedName;
        private string assemblyName;
        private Type type = null;

        public SerializableType(System.Type type)
        {
            this.type = type;
            name = type.Name;
            assemblyQualifiedName = type.AssemblyQualifiedName;
            assemblyName = type.Assembly.FullName;
        }

        public static bool operator ==(SerializableType a, SerializableType b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (((object)a == null) ^ ((object)b == null))
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(SerializableType a, SerializableType b)
        {
            return !(a == b);
        }

        public static implicit operator SerializableType(Type type) => new SerializableType(type);

        public static implicit operator Type(SerializableType type) => type.GetType();

        public override bool Equals(object obj)
        {
            if (obj is SerializableType == false)
            {
                return false;
            }
            return Equals(obj as SerializableType);
        }

        public bool Equals(SerializableType obj)
        {
            return obj.GetType().Equals(type);
        }

        public override int GetHashCode()
        {
            return type.GetHashCode();
        }

        public new System.Type GetType()
        {
            if (type == null)
            {
                type = System.Type.GetType(assemblyQualifiedName);
            }
            return type;
        }
    }
}