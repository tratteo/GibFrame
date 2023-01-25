using UnityEngine;

namespace GibFrame.Audio
{
    [System.Serializable]
    internal class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 0.9f;
        [Range(0f, 3f)]
        public float pitch = 1f;
        public bool loop;
        [HideInInspector] public AudioSource source;
        [HideInInspector] public float currentVolume;
        [HideInInspector] public bool isSmoothing;
    }
}