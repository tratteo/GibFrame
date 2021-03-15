//Copyright (c) matteo
//UnityUtils.cs - com.tratteo.gibframe

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GibFrame.Utils
{
    public class UnityUtils
    {
        public static List<T> GetInterfacesOfType<T>()
        {
            List<T> interfaces = new List<T>();
            MonoBehaviour[] monoBehaviours = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();
            foreach (MonoBehaviour item in monoBehaviours)
            {
                if (item is T)
                {
                    T[] elems = item.GetComponents<T>();
                    foreach (T e in elems)
                    {
                        interfaces.Add(e);
                    }
                }
            }
            return interfaces;
        }

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

        [Obsolete("Use the Unity Resources API instead")]
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

        [Obsolete("Use the Unity Resources API instead")]
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

        [Obsolete("Use the Unity Resources API instead")]
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
    }
}
