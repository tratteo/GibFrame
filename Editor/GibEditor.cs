using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    public static class GibEditor
    {
        /// <summary>
        /// </summary>
        /// <param name="root"> </param>
        /// <returns> All <see cref="UnityEngine.Object"/> in the Asset folder </returns>
        public static List<Object> GetObjectsInAssets(string root = "")
        {
            var completePath = string.IsNullOrWhiteSpace(root) ? Application.dataPath : $"{Application.dataPath}{Path.DirectorySeparatorChar}{root}";
            if (!Directory.Exists(completePath)) return new List<Object>();

            var sel = new List<Object>();
            var relativePath = string.IsNullOrWhiteSpace(root) ? "Assets" : root;
            var assetsPrefix = string.IsNullOrWhiteSpace(root) ? string.Empty : $"Assets{Path.DirectorySeparatorChar}";

            var fileEntries = Directory.GetFiles(completePath);
            foreach (var file in fileEntries)
            {
                var fileName = Path.GetFileName(file);
                var asset = AssetDatabase.LoadMainAssetAtPath($"{assetsPrefix}{relativePath}{Path.DirectorySeparatorChar}{fileName}");
                if (!asset) continue;
                sel.Add(asset);
            }
            var dirs = Directory.GetDirectories(completePath);
            foreach (var dir in dirs)
            {
                var dirInfo = new DirectoryInfo(dir);
                var localPath = string.IsNullOrWhiteSpace(root) ? dirInfo.Name : $"{root}{Path.DirectorySeparatorChar}{dirInfo.Name}";
                sel.AddRange(GetObjectsInAssets(localPath));
            }
            return sel;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="root"> </param>
        /// <returns> All behaviours of type T in the Asset folder. T can be either a <see cref="MonoBehaviour"/> or a <see cref="ScriptableObject"/> </returns>
        public static List<T> GetAllBehavioursInAsset<T>(string root = "") where T : Object
        {
            var objs = GetObjectsInAssets(root);
            var behaviours = new List<T>();
            foreach (var obj in objs)
            {
                if (typeof(T).IsSubclassOf(typeof(ScriptableObject)) || typeof(T).Equals(typeof(Object)))
                {
                    if (obj is T so)
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

        /// <summary>
        /// </summary>
        /// <returns> All the <see cref="Object"/> in the current scene </returns>
        public static Object[] GetObjectsInScene() => Object.FindObjectsOfType<Object>();
    }
}