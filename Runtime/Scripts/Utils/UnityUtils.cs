// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : UnityUtils.cs
//
// All Rights Reserved

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GibFrame
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

        public static Vector3 GetCoordinateCenter(params Vector3[] positions)
        {
            int length = positions.Length;
            float x = 0;
            float y = 0;
            float z = 0;
            foreach (Vector3 obj in positions)
            {
                x += obj.x;
                y += obj.y;
                z += obj.z;
            }
            x /= length;
            y /= length;
            z /= length;
            return new Vector3(x, y, z);
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
