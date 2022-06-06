// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : GuardedAttribute.cs
//
// All Rights Reserved

using System;
using UnityEngine;

namespace GibFrame.Meta
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class GuardedAttribute : PropertyAttribute
    {
        public enum MissingValueGravity
        { Info, Warning, Error }

        public string Message { get; private set; }

        public MissingValueGravity Gravity { get; private set; }

        public GuardedAttribute() : this(string.Empty, MissingValueGravity.Warning)
        {
        }

        public GuardedAttribute(MissingValueGravity gravity) : this(string.Empty, gravity)
        {
        }

        public GuardedAttribute(string message) : this(message, MissingValueGravity.Warning)
        {
        }

        public GuardedAttribute(string message, MissingValueGravity gravity)
        {
            Message = message;
            Gravity = gravity;
        }
    }
}
