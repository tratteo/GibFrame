using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GibFrame.Editor
{
    public static class GibEditor
    {
        /// <summary>
        /// </summary>
        /// <param name="root"> </param>
        /// <param name="exclusion"> Folders name to exclude from the query </param>
        /// <returns> All <see cref="UnityEngine.Object"/> in the specified path </returns>
        public static List<UnityEngine.Object> GetUnityObjectsAtPath(string path, params string[] exclusion)
        {
            if (!Directory.Exists(path)) return new List<UnityEngine.Object>();
            var inAsset = path.Contains(Application.dataPath);
            if (inAsset)
            {
                path = path.Replace(Application.dataPath, string.Empty);
                path = string.IsNullOrWhiteSpace(path) ? "Assets" : $"Assets{path}";
            }
            var exclusionList = exclusion.ToList();
            var sel = new List<UnityEngine.Object>();
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
                sel.AddRange(GetUnityObjectsAtPath(localPath));
            }
            return sel;
        }

        /// <summary>
        /// </summary>
        /// <param name="root"> The root path to start searching from </param>
        /// <param name="exclusion"> Folders name to exclude from the query </param>
        /// <returns> All <see cref="UnityEngine.Object"/> in the Asset folder </returns>
        public static List<UnityEngine.Object> GetUnityObjectsInAssets(string root = "", params string[] exclusion)
        {
            var compositePath = string.IsNullOrWhiteSpace(root) ? Application.dataPath : $"{Application.dataPath}{Path.AltDirectorySeparatorChar}{root}";
            return GetUnityObjectsAtPath(compositePath, exclusion);
        }

        /// <summary>
        ///   Applied to all <see cref="UnityEngine.Object"/> in the specified <i> path </i> folder. <inheritdoc cref="Gib.GetAllBehaviours{T}(UnityEngine.Object[])"/>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="root"> </param>
        /// <returns> </returns>
        public static List<T> GetAllBehavioursAtPath<T>(string path) where T : UnityEngine.Object => Gib.GetAllBehaviours<T>(GetUnityObjectsAtPath(path).ToArray());

        /// <summary>
        ///   Applied to all <see cref="UnityEngine."/> in the <i> Asset </i> folder. <inheritdoc cref="Gib.GetAllBehaviours{T}(UnityEngine.Object[])"/>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="root"> </param>
        /// <returns> </returns>
        public static List<T> GetAllBehavioursInAsset<T>(string root = "") where T : UnityEngine.Object => Gib.GetAllBehaviours<T>(GetUnityObjectsInAssets(root).ToArray());

        /// <summary>
        ///   Execute an <see cref="Action"/> on all the <see cref="GameObject"/> in the specified scene
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="scenePath"> </param>
        /// <param name="action"> </param>
        /// <returns> The number of <see cref="GameObject"/> the <see cref="Action"/> has been run on </returns>
        public static int ExecuteForGameObjectsInScene(string scenePath, Action<GameObject> action = null)
        {
            var count = 0;
            var openedScene = SceneManager.GetActiveScene();
            var openedScenePath = openedScene.path;
            var isOpenedScene = openedScenePath.Equals(scenePath);
            if (!isOpenedScene)
            {
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            }
            var sceneRef = isOpenedScene ? openedScene : EditorSceneManager.OpenScene(scenePath);

            var scenesObjs = sceneRef.GetRootGameObjects();
            foreach (var obj in scenesObjs)
            {
                var children = obj.GetComponentsInChildren<Transform>(true);
                foreach (var t in children)
                {
                    action?.Invoke(t.gameObject);
                    count++;
                }
            }
            if (!isOpenedScene) EditorSceneManager.OpenScene(openedScenePath);
            return count;
        }

        /// <summary>
        ///   Execute an <see cref="Action"/> on all the components of type T in the specified scene
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="scenePath"> </param>
        /// <param name="action"> </param>
        /// <returns> The number of components the <see cref="Action"/> has been run on </returns>
        public static int ExecuteForComponentsInScene<T>(string scenePath, Action<T> action = null) where T : Component
        {
            var count = 0;
            ExecuteForGameObjectsInScene(scenePath, obj =>
            {
                var comps = obj.GetComponents<T>();
                foreach (var c in comps) action?.Invoke(c);
                count += comps.Length;
            });

            return count;
        }
    }
}