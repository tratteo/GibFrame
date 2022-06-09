using UnityEngine;

namespace GibFrame
{
    [System.Serializable]
    public class SceneReference
    {
        [SerializeField] private string path;
        [SerializeField] private string name;

        public string Name => name;

        public string Path => path;
    }
}