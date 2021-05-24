// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.SceneManagement : SceneUtil.cs
//
// All Rights Reserved

using UnityEngine;
using UnityEngine.SceneManagement;

namespace GibFrame.SceneManagement
{
    internal static class SceneUtil
    {
        public const string ASYNCLOADER_SCENE_NAME = "AsyncLoaderScene";

        internal static void LoadScene(string name)
        {
            Time.timeScale = 1F;
            int index = SceneUtility.GetBuildIndexByScenePath(name);
            if (index != -1)
            {
                SceneManager.LoadScene(index);
            }
        }

        internal static void LoadSceneAsynchronously(string name)
        {
            int index = SceneUtility.GetBuildIndexByScenePath(name);
            if (index != -1)
            {
                Time.timeScale = 1F;
                int asyncSceneIndex = SceneUtility.GetBuildIndexByScenePath(ASYNCLOADER_SCENE_NAME);
                AsyncLoadIndexSaver.SetIndexToPreload(index);
                SceneManager.LoadScene(asyncSceneIndex);
            }
        }

        internal static void LoadSceneAsynchronously(int sceneIndex)
        {
            LoadSceneAsynchronously(SceneUtility.GetScenePathByBuildIndex(sceneIndex));
        }
    }
}
