// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.SceneManagement : AsyncLoader.cs
//
// All Rights Reserved

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GibFrame.SceneManagement
{
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
