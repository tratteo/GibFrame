﻿// Copyright (c) Matteo Beltrame
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
    public class Gib
    {
        public static T FindObjectOfTypeWithName<T>(string name, bool inactive = false) where T : Component
        {
            var transforms = UnityEngine.Object.FindObjectsOfType<T>(inactive);
            return Array.Find(transforms, (elem) => elem.name.Equals(name));
        }

        public static List<T> GetInterfacesOfType<T>(bool inactive = false) where T : class
        {
            var interfaces = new List<T>();
            var objects = UnityEngine.Object.FindObjectsOfType<GameObject>(inactive);
            foreach (var obj in objects)
            {
                var elems = obj.GetComponents<T>();
                interfaces.AddRange(elems);
            }
            return interfaces;
        }

        public static T GetFirstInterfaceOfType<T>(bool inactive = false) where T : class
        {
            var monoBehaviours = UnityEngine.Object.FindObjectsOfType<GameObject>(inactive);
            foreach (var item in monoBehaviours)
            {
                if (item.TryGetComponent<T>(out var elem))
                {
                    return elem;
                }
            }
            return default;
        }

        public static Vector3 GetCoordinateCenter(params Vector3[] positions)
        {
            var length = positions.Length;
            float x = 0;
            float y = 0;
            float z = 0;
            foreach (var obj in positions)
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
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            var results = new List<RaycastResult>();
            if (EventSystem.current is not null)
            {
                EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                return results.Count > 0;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Missing event system in scene");
            }
            return false;
        }
    }
}