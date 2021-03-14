//Copyright (c) matteo
//SceneLoader.cs - com.tratteo.gibframe

using UnityEngine;
using UnityEngine.SceneManagement;

namespace GibFrame.SceneManagement
{
    /// <summary>
    ///   Utilities class to manage the scene management. Attach this component to a GameObject and use the EventTrigger component to
    ///   trigger the functions
    /// </summary>
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