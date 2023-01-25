using System;
using UnityEngine;

namespace GibFrame.Meta
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class GuidAttribute : PropertyAttribute
    {
        public bool Readonly { get; set; } = true;

        public bool Resettable { get; set; } = false;

        public GuidAttribute()
        {
        }
    }
}