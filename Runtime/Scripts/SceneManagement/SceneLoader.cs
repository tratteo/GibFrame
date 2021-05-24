// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.SceneManagement : SceneLoader.cs
//
// All Rights Reserved

using UnityEngine;
using UnityEngine.SceneManagement;

namespace GibFrame.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(string name)
        {
            SceneUtil.LoadScene(name);
        }

        public void LoadSceneAsynchronously(string name)
        {
            SceneUtil.LoadSceneAsynchronously(name);
        }

        public void ReloadCurrentSceneAsynchronously()
        {
            SceneUtil.LoadSceneAsynchronously(SceneManager.GetActiveScene().name);
        }
    }
}
