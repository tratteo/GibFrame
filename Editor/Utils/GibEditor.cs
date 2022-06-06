using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GibFrame.Editor
{
    public static class GibEditor
    {
        /// <summary>
        /// </summary>
        /// <param name="root"> </param>
        /// <param name="exclusion"> Folders name to exclude from the query </param>
        /// <returns> All <see cref="UnityEngine.Object"/> in the specified path </returns>
        public static List<Object> GetObjectsAtPath(string path, params string[] exclusion)
        {
            if (!Directory.Exists(path)) return new List<Object>();
            var inAsset = path.Contains(Application.dataPath);
            if (inAsset)
            {
                path = path.Replace(Application.dataPath, string.Empty);
                path = string.IsNullOrWhiteSpace(path) ? "Assets" : $"Assets{path}";
            }
            var exclusionList = exclusion.ToList();
            var sel = new List<Object>();
            var fileEntries = Directory.GetFiles(path);
            foreach (var file in fileEntries)
            {
                var fileName = Path.GetFileName(file);
                var asset = AssetDatabase.LoadMainAssetAtPath($"{path}{Path.DirectorySeparatorChar}{fileName}");
                if (!asset) continue;
                sel.Add(asset);
            }
            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                var dirInfo = new DirectoryInfo(dir);
                if (exclusionList.Contains(dirInfo.Name)) continue;
                var localPath = $"{path}{Path.DirectorySeparatorChar}{dirInfo.Name}";
                sel.AddRange(GetObjectsAtPath(localPath));
            }
            return sel;
        }

        /// <summary>
        /// </summary>
        /// <param name="root"> The root path to start searching from </param>
        /// <param name="exclusion"> Folders name to exclude from the query </param>
        /// <returns> All <see cref="UnityEngine.Object"/> in the Asset folder </returns>
        public static List<Object> GetObjectsInAssets(string root = "", params string[] exclusion)
        {
            var compositePath = string.IsNullOrWhiteSpace(root) ? Application.dataPath : $"{Application.dataPath}{Path.AltDirectorySeparatorChar}{root}";
            return GetObjectsAtPath(compositePath, exclusion);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="path"> </param>
        /// <returns> All behaviours of type T in the specified path. T can be either a <see cref="MonoBehaviour"/> or a <see cref="ScriptableObject"/> </returns>
        public static List<T> GetAllBehavioursAtPath<T>(string path) where T : Object => GetBehaviours<T>(GetObjectsAtPath(path));

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="root"> </param>
        /// <returns> All behaviours of type T in the Asset folder. T can be either a <see cref="MonoBehaviour"/> or a <see cref="ScriptableObject"/> </returns>
        public static List<T> GetAllBehavioursInAsset<T>(string root = "") where T : Object => GetBehaviours<T>(GetObjectsInAssets(root));

        /// <summary>
        /// </summary>
        /// <returns> All the <see cref="Object"/> in the current scene </returns>
        public static Object[] GetObjectsInScene() => Object.FindObjectsOfType<Object>();

        public static List<Object> GetBehaviours(IEnumerable<Object> objs)
        {
            var behaviours = new List<Object>();
            foreach (var obj in objs)
            {
                if (obj is ScriptableObject so)
                {
                    behaviours.Add(so);
                }
                else if (obj is GameObject gObj)
                {
                    behaviours.AddRange(gObj.GetComponents<MonoBehaviour>());
                }
            }
            return behaviours;
        }

        public static List<T> GetBehaviours<T>(IEnumerable<Object> objs)
        {
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
    }
}