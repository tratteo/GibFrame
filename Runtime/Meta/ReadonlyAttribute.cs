// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : ReadonlyAttribute.cs
//
// All Rights Reserved

using System;
using UnityEngine;

namespace GibFrame.Meta
{
    /// <summary>
    ///   Mark the serialized element as readonly, prevent changes happening from the inspector
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ReadonlyAttribute : PropertyAttribute
    {
    }
}