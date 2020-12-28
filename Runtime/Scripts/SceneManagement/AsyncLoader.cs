// Copyright (c) 2020 Matteo Beltrame

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GibFrame.SceneManagement
{
    // The async loader works like this: Add a SceneLoader in a scene, when calling (from a button for example)
    // LoadSceneAsynchronously(string name), the SceneLoader will resolve the name and find the scene index then it will save it statically
    // into the AsyncLoadIndexSaver and will load the AsyncScene. When loaded the AsyncLoader will load from the static class the index to
    // load and will load the corresponding scene asynchronously. Remember only to add the AsyncScene to the build scenes.
    public class AsyncLoader : MonoBehaviour
    {
        public Image loadingBar;
        private int indexToPreload;

        private void Start()
        {
            indexToPreload = AsyncLoadIndexSaver.GetSceneIndexToPreload();
            StartCoroutine(LoadSceneAsync());
        }

        private IEnumerator LoadSceneAsync()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(indexToPreload);
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                loadingBar.fillAmount = progress;
                yield return null;
            }
        }
    }
}