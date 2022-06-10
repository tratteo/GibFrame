// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : UnityUtils.cs
//
// All Rights Reserved

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GibFrame
{
    public class Gib
    {
        /// <summary>
        ///   For each object:
        ///   <list>
        ///     <item>
        ///       <description> - If it is a <see cref="GameObject"/>, retrieve all components of type T </description>
        ///     </item>
        ///     <item>
        ///       <description> - If it is a <see cref="ScriptableObject"/>, retrieve it if it's of type T </description>
        ///     </item>
        ///   </list>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="objs"> </param>
        /// <returns> </returns>
        public static List<T> GetAllBehaviours<T>(params UnityEngine.Object[] objs) where T : class
        {
            var behaviours = new List<T>();
            foreach (var obj in objs)
            {
                if (typeof(T).IsSubclassOf(typeof(ScriptableObject)))
                {
                    if (typeof(T).Equals(typeof(UnityEngine.Object)))
                    {
                        behaviours.Add(obj as T);
                    }
                    else if (obj is T so)
                    {
                        behaviours.Add(so);
                    }
                }
                else if (typeof(T).IsSubclassOf(typeof(Component)) && obj is GameObject gObj)
                {
                    behaviours.AddRange(gObj.GetComponents<T>());
                }
            }
            return behaviours;
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