﻿// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : ReadonlyAttribute.cs
//
// All Rights Reserved

using System;
using UnityEngine;

namespace GibFrame
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ReadonlyAttribute : PropertyAttribute
    {
    }
}
