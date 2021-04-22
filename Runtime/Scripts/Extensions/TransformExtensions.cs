using System.Collections.Generic;
using UnityEngine;

namespace GibFrame.Extensions
{
    public static class TransformExtensions
    {
        public static T GetFirstComponentInChildrenWithTag<T>(this Transform parent, string tag, bool inactive)
        {
            Transform[] transforms = parent.GetComponentsInChildren<Transform>(inactive);

            for (int i = 1; i < transforms.Length; i++)
            {
                if (transforms[i].tag.Equals(tag))
                {
                    T component = transforms[i].gameObject.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
            return default(T);
        }

        public static Transform GetFirstChildWithName(this Transform parent, string name, bool inactive)
        {
            Transform[] transforms = parent.GetComponentsInChildren<Transform>(inactive);
            foreach (Transform transform in transforms)
            {
                if (transform.name.Equals(name))
                {
                    return transform;
                }
            }
            return null;
        }

        public static T GetFirstComponentInParentsWithTag<T>(this Transform parent, string tag, bool inactive)
        {
            Transform[] transforms = parent.GetComponentsInParent<Transform>(inactive);
            for (int i = 1; i < transforms.Length; i++)
            {
                if (transforms[i].tag.Equals(tag))
                {
                    T component = transforms[i].gameObject.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
            return default(T);
        }

        public static Transform GetFirstChildWithComponent<T>(this Transform parent)
        {
            T parentComponent = parent.GetComponent<T>(), childComponent;
            foreach (Transform transform in parent.transform)
            {
                childComponent = transform.GetComponent<T>();
                if (childComponent != null && !childComponent.Equals(parentComponent))
                {
                    return transform;
                }
            }
            return null;
        }

        public static List<Transform> GetGameObjectsInChildrenWithTag(this Transform parent, string tag, bool inactive)
        {
            List<Transform> objs = new List<Transform>();
            Transform[] transforms = parent.GetComponentsInChildren<Transform>(inactive);
            for (int i = 1; i < transforms.Length; i++)
            {
                if (transforms[i].tag == tag)
                {
                    objs.Add(transforms[i]);
                }
            }
            return objs;
        }

        public static List<Transform> GetGameObjectsInParentsWithTag(this Transform parent, string tag, bool inactive)
        {
            List<Transform> objs = new List<Transform>();
            Transform[] transforms = parent.GetComponentsInParent<Transform>(inactive);
            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].tag == tag)
                {
                    objs.Add(transforms[i]);
                }
            }
            return objs;
        }

        public static Transform GetFirstGameObjectInParentWithTag(this Transform parent, string tag, bool inactive)
        {
            Transform[] transforms = parent.GetComponentsInParent<Transform>(inactive);
            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].tag == tag)
                {
                    return transforms[i];
                }
            }
            return null;
        }

        public static Transform GetFirstGameObjectInChildrenWithTag(this Transform parent, string tag, bool inactive)
        {
            Transform[] transforms = parent.GetComponentsInChildren<Transform>(inactive);
            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].tag == tag)
                {
                    return transforms[i];
                }
            }
            return null;
        }

        public static T GetFirstComponentInChildrenWithName<T>(this Transform parent, string name, bool inactive)
        {
            Transform[] transforms = parent.GetComponentsInChildren<Transform>(inactive);
            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].name.Equals(name))
                {
                    T component = transforms[i].gameObject.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
            return default(T);
        }

        public static T GetFirstComponentInParentWithName<T>(this Transform parent, string name, bool inactive)
        {
            Transform[] transforms = parent.GetComponentsInParent<Transform>(inactive);
            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].name.Equals(name))
                {
                    T component = transforms[i].gameObject.GetComponent<T>();
                    if (component != null)
                    {
                        return component;
                    }
                }
            }
            return default(T);
        }
    }
}
