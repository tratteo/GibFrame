// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : TransformExtensions.cs
//
// All Rights Reserved

using System.Collections.Generic;
using UnityEngine;

namespace GibFrame.Extensions
{
    public static class TransformExtensions
    {
        public static T GetFirstComponentInParentsWithTag<T>(this Transform parent, string tag, bool inactive, bool includeParent = false) where T : class
        {
            var transforms = parent.GetComponentsInParent<Transform>(inactive);
            for (var i = includeParent ? 0 : 1; i < transforms.Length; i++)
            {
                if (transforms[i].CompareTag(tag))
                {
                    if (transforms[i].gameObject.TryGetComponent<T>(out var component))
                    {
                        return component;
                    }
                }
            }
            return default;
        }

        public static List<Transform> GetGameObjectsInParentsWithTag(this Transform parent, string tag, bool inactive, bool includeParent = false)
        {
            var objs = new List<Transform>();
            var transforms = parent.GetComponentsInParent<Transform>(inactive);
            for (var i = includeParent ? 0 : 1; i < transforms.Length; i++)
            {
                if (transforms[i].CompareTag(tag))
                {
                    objs.Add(transforms[i]);
                }
            }
            return objs;
        }

        public static Transform GetFirstGameObjectInParentsWithTag(this Transform parent, string tag, bool inactive, bool includeParent = false)
        {
            var transforms = parent.GetComponentsInParent<Transform>(inactive);
            for (var i = includeParent ? 0 : 1; i < transforms.Length; i++)
            {
                if (transforms[i].CompareTag(tag))
                {
                    return transforms[i];
                }
            }
            return null;
        }

        public static T GetFirstComponentInChildrenWithTag<T>(this Transform parent, string tag, bool inactive, bool includeParent = false) where T : class
        {
            var transforms = parent.GetComponentsInChildren<Transform>(inactive);
            for (var i = includeParent ? 0 : 1; i < transforms.Length; i++)
            {
                if (transforms[i].CompareTag(tag))
                {
                    if (transforms[i].gameObject.TryGetComponent<T>(out var component))
                    {
                        return component;
                    }
                }
            }
            return default(T);
        }

        public static Transform GetFirstChildWithName(this Transform parent, string name, bool inactive, bool includeParent = false)
        {
            var transforms = parent.GetComponentsInChildren<Transform>(inactive);
            for (var i = includeParent ? 0 : 1; i < transforms.Length; i++)
            {
                var transform = transforms[i];
                if (transform.name.Equals(name))
                {
                    return transform;
                }
            }
            return null;
        }

        public static List<Transform> GetGameObjectsInChildrenWithTag(this Transform parent, string tag, bool inactive, bool includeParent = false)
        {
            var objs = new List<Transform>();
            var transforms = parent.GetComponentsInChildren<Transform>(inactive);
            for (var i = includeParent ? 0 : 1; i < transforms.Length; i++)
            {
                if (transforms[i].CompareTag(tag))
                {
                    objs.Add(transforms[i]);
                }
            }
            return objs;
        }

        public static Transform GetFirstGameObjectInChildrenWithTag(this Transform parent, string tag, bool inactive, bool includeParent = false)
        {
            var transforms = parent.GetComponentsInChildren<Transform>(inactive);
            for (var i = includeParent ? 0 : 1; i < transforms.Length; i++)
            {
                if (transforms[i].CompareTag(tag))
                {
                    return transforms[i];
                }
            }
            return null;
        }

        public static T GetFirstComponentInChildrenWithName<T>(this Transform parent, string name, bool inactive, bool includeParent = false) where T : class
        {
            var transforms = parent.GetComponentsInChildren<Transform>(inactive);
            for (var i = includeParent ? 0 : 1; i < transforms.Length; i++)
            {
                if (transforms[i].name.Equals(name))
                {
                    if (transforms[i].gameObject.TryGetComponent<T>(out var component))
                    {
                        return component;
                    }
                }
            }
            return default;
        }

        public static T GetFirstComponentInParentWithName<T>(this Transform parent, string name, bool inactive, bool includeParent) where T : class
        {
            var transforms = parent.GetComponentsInParent<Transform>(inactive);
            for (var i = includeParent ? 0 : 1; i < transforms.Length; i++)
            {
                if (transforms[i].name.Equals(name))
                {
                    if (transforms[i].gameObject.TryGetComponent<T>(out var component))
                    {
                        return component;
                    }
                }
            }
            return default;
        }
    }
}