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
        public static T FindObjectOfTypeWithName<T>(string name, bool inactive = false) where T : Component
        {
            T[] transforms = UnityEngine.Object.FindObjectsOfType<T>(inactive);
            return Array.Find(transforms, (elem) => elem.name.Equals(name));
        }

        public static List<T> GetInterfacesOfType<T>(bool inactive = false)
        {
            List<T> interfaces = new List<T>();
            MonoBehaviour[] monoBehaviours = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>(inactive);
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

        public static T GetFirstInterfaceOfType<T>(bool inactive = false)
        {
            MonoBehaviour[] monoBehaviours = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>(inactive);
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
