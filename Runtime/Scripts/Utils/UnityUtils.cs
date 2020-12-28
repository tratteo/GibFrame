// Copyright (c) 2020 Matteo Beltrame

using System;
using System.Collections;
using System.Collections.Generic;
using GibFrame.Utils.Callbacks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace GibFrame.Utils
{
    public class UnityUtils
    {
        public static T GetFirstInterfaceOfType<T>()
        {
            MonoBehaviour[] monoBehaviours = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour item in monoBehaviours)
            {
                if (item is T)
                {
                    return item.gameObject.GetComponent<T>();
                }
            }
            return default;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="parent"> </param>
        /// <param name="tag"> </param>
        /// <param name="inactive"> </param>
        /// <returns> First matching Type component in a child that has the specified tag </returns>
        public static T GetFirstComponentInChildrenWithTag<T>(GameObject parent, string tag, bool inactive)
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

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="parent"> </param>
        /// <param name="tag"> </param>
        /// <param name="inactive"> </param>
        /// <returns> First matching Type component in a parent that has the specified tag </returns>
        public static T GetFirstComponentInParentsWithTag<T>(GameObject parent, string tag, bool inactive)
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

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="parent"> </param>
        /// <returns> First child GameObject that has a matching type component </returns>
        public static GameObject GetFirstChildWithComponent<T>(GameObject parent)
        {
            T parentComponent = parent.GetComponent<T>(), childComponent;
            foreach (Transform transform in parent.transform)
            {
                childComponent = transform.GetComponent<T>();
                if (childComponent != null && !childComponent.Equals(parentComponent))
                {
                    return transform.gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="parent"> </param>
        /// <param name="tag"> </param>
        /// <param name="inactive"> </param>
        /// <returns> List of all children GameObjects with the specified tag, parent excluded </returns>
        public static List<GameObject> GetGameObjectsInChildrenWithTag(GameObject parent, string tag, bool inactive)
        {
            List<GameObject> objs = new List<GameObject>();
            Transform[] transforms = parent.GetComponentsInChildren<Transform>(inactive);
            for (int i = 1; i < transforms.Length; i++)
            {
                if (transforms[i].tag == tag)
                {
                    objs.Add(transforms[i].gameObject);
                }
            }
            return objs;
        }

        /// <summary>
        /// </summary>
        /// <param name="parent"> </param>
        /// <param name="tag"> </param>
        /// <param name="inactive"> </param>
        /// <returns> List of all parents GameObjects with the specified tag </returns>
        public static List<GameObject> GetGameObjectsInParentsWithTag(GameObject parent, string tag, bool inactive)
        {
            List<GameObject> objs = new List<GameObject>();
            Transform[] transforms = parent.GetComponentsInParent<Transform>(inactive);
            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].tag == tag)
                {
                    objs.Add(transforms[i].gameObject);
                }
            }
            return objs;
        }

        /// <summary>
        /// </summary>
        /// <param name="parent"> </param>
        /// <param name="tag"> </param>
        /// <param name="inactive"> </param>
        /// <returns> First occurence of a parent with the selected tag </returns>
        public static GameObject GetFirstGameObjectInParentWithTag(GameObject parent, string tag, bool inactive)
        {
            Transform[] transforms = parent.GetComponentsInParent<Transform>(inactive);
            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].tag == tag)
                {
                    return transforms[i].gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="parent"> </param>
        /// <param name="tag"> </param>
        /// <param name="inactive"> </param>
        /// <returns> First occurence of a child with the selected tag </returns>
        public static GameObject GetFirstGameObjectInChildrenWithTag(GameObject parent, string tag, bool inactive)
        {
            Transform[] transforms = parent.GetComponentsInChildren<Transform>(inactive);
            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].tag == tag)
                {
                    return transforms[i].gameObject;
                }
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="parent"> </param>
        /// <param name="name"> </param>
        /// <param name="inactive"> </param>
        /// <returns> First Component retrieved from the first occurrece of a child with a matching name </returns>
        public static T GetFirstComponentInChildrenWithName<T>(GameObject parent, string name, bool inactive)
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

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="parent"> </param>
        /// <param name="name"> </param>
        /// <param name="inactive"> </param>
        /// <returns> First Component retrieved from the first occurrece of a parent with a matching name </returns>
        public static T GetFirstComponentInParentWithName<T>(GameObject parent, string name, bool inactive)
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

        /// <summary>
        ///   Read from the relative path inside Resources folder and retrieve from each element the component T. If none is found an array
        ///   of length 0 is assigned
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="array"> </param>
        /// <param name="path"> </param>
        public static void ReadAndCastAllComponents<T>(out T[] array, string path)
        {
            UnityEngine.Object[] all = Resources.LoadAll(path);
            if (all == null)
            {
                array = new T[0];
                return;
            }
            int length = all.Length;
            array = new T[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = (all[i] as GameObject).GetComponent<T>();
            }
        }

        /// <summary>
        ///   Read from the relative path inside Resources folder and retrieve the T component. If no element or component is found, null is returned
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="obj"> </param>
        /// <param name="path"> </param>
        public static void ReadAndCastComponent<T>(out T obj, string path)
        {
            UnityEngine.Object res = Resources.Load<GameObject>(path);
            if (res == null)
            {
                obj = default(T);
                return;
            }
            else
            {
                obj = (res as GameObject).GetComponent<T>();
            }
        }

        public static void ReadAllAs<T>(out T[] array, string path) where T : class
        {
            UnityEngine.Object[] all = Resources.LoadAll(path);
            if (all == null)
            {
                array = new T[0];
                return;
            }
            int length = all.Length;
            array = new T[length];
            for (int i = 0; i < length; i++)
            {
                array[i] = (all[i] as T);
            }
        }

        public static void ReadGameObject(out GameObject obj, string path)
        {
            obj = Resources.Load<GameObject>(path);
        }

        /// <summary>
        ///   Toggle the visibility of a GameObject and all of his children
        /// </summary>
        public static void MakeGameObjectVisible(GameObject obj, bool state)
        {
            if (obj == null) return;
            Transform[] transforms = obj.GetComponentsInChildren<Transform>();
            Renderer renderer;
            int length = transforms.Length;
            for (int i = 0; i < length; i++)
            {
                renderer = transforms[i].gameObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.enabled = state;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <returns> Whether the mouse or a touch are overlapping a game object </returns>
        public static bool IsAnyPointerOverGameObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        public static void ExecuteWithDelay(MonoBehaviour context, AbstractCallback Callback, float duration, bool realtime)
        {
            context.StartCoroutine(ExecuteWithDelay_C(Callback, duration, realtime));
        }

        private static IEnumerator ExecuteWithDelay_C(AbstractCallback Callback, float duration, bool realtime)
        {
            if (realtime)
            {
                yield return new WaitForSecondsRealtime(duration);
            }
            else
            {
                yield return new WaitForSeconds(duration);
            }
            Callback?.Invoke();
        }
    }
}