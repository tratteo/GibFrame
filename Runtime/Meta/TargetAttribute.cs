using System;
using UnityEngine;

namespace GibFrame.Meta
{
    public static class TargetAttributeExtensions
    {
        public static bool SatisfiesTargetAttribute<T>(this T type, GameObject parent)
        {
            var attributes = Attribute.GetCustomAttributes(type.GetType(), typeof(TargetAttribute));
            foreach (var att in attributes)
            {
                if (att is TargetAttribute target)
                {
                    var component = target.IncludeChildren ? parent.GetComponentInChildren(target.TargetType) : parent.GetComponent(target.TargetType);
                    if (!component)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class TargetAttribute : Attribute
    {
        public bool IncludeChildren { get; set; }

        public Type TargetType { get; private set; }

        public TargetAttribute(Type type)
        {
            TargetType = type;
            IncludeChildren = false;
        }
    }
}