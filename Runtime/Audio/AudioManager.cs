using System;
using System.Collections;

using UnityEngine;

namespace GibFrame.Audio
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] private Sound[] sounds = null;
        [SerializeField] private Sound[] musics = null;

        public void PlaySound(string name)
        {
            if (!IsSoundValid(name)) return;
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.Play();
        }

        public void StopSound(string name)
        {
            if (!IsSoundValid(name)) return;
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.Stop();
        }

        public bool IsSoundPlaying(string name)
        {
            if (!IsSoundValid(name)) return false;
            Sound s = Array.Find(sounds, sound => sound.name == name);
            return s.source.isPlaying;
        }

        public void SmoothOutSound(string name, float duration)
        {
            if (!IsSoundValid(name)) return;
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s.source.isPlaying)
            {
                StartCoroutine(SmoothOutSound_C(s, duration));
            }
        }

        public void SmoothInSound(string name, float duration)
        {
            if (!IsSoundValid(name)) return;
            Sound s = Array.Find(sounds, sound => sound.name == name);
            StartCoroutine(SmoothInSound_C(s, duration));
        }

        public void ChangeSoundsVolume(float volume)
        {
            foreach (Sound s in sounds)
            {
                s.currentVolume = s.volume * volume;
                if (!s.isSmoothing)
                {
                    s.source.volume = s.currentVolume;
                }
            }
        }

        public void PlayMusic(string name)
        {
            if (!IsMusicValid(name)) return;
            Sound s = Array.Find(musics, sound => sound.name == name);
            s.source.Play();
        }

        public void StopMusic(string name)
        {
            if (!IsMusicValid(name)) return;
            Sound s = Array.Find(musics, sound => sound.name == name);
            s.source.Stop();
        }

        public bool IsMusicPlaying(string name)
        {
            if (!IsMusicValid(name)) return false;
            Sound s = Array.Find(musics, music => music.name == name);
            return s != null && s.source.isPlaying;
        }

        public void SmoothOutMusic(string name, float duration)
        {
            if (!IsMusicValid(name)) return;
            Sound s = Array.Find(musics, sound => sound.name == name);
            if (s.source.isPlaying)
            {
                StartCoroutine(SmoothOutSound_C(s, duration));
            }
        }

        public void SmoothInMusic(string name, float duration)
        {
            if (!IsMusicValid(name)) return;
            Sound s = Array.Find(musics, sound => sound.name == name);
            StartCoroutine(SmoothInSound_C(s, duration));
        }

        public void ChangeMusicVolume(float volume)
        {
            foreach (Sound s in musics)
            {
                s.currentVolume = s.volume * volume;
                if (!s.isSmoothing)
                {
                    s.source.volume = s.currentVolume;
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();

            int length = sounds.Length;
            for (int i = 0; i < length; i++)
            {
                sounds[i].source = gameObject.AddComponent<AudioSource>();
                sounds[i].source.clip = sounds[i].clip;
                sounds[i].source.volume = sounds[i].volume;
                sounds[i].currentVolume = sounds[i].volume;
                sounds[i].source.pitch = sounds[i].pitch;
                sounds[i].source.loop = sounds[i].loop;
            }

            length = musics.Length;
            for (int i = 0; i < length; i++)
            {
                musics[i].source = gameObject.AddComponent<AudioSource>();
                musics[i].source.clip = musics[i].clip;
                musics[i].source.volume = musics[i].volume;
                musics[i].currentVolume = musics[i].volume;
                musics[i].source.pitch = musics[i].pitch;
                musics[i].source.loop = musics[i].loop;
            }
        }

        private bool IsSoundValid(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                UnityEngine.Debug.LogError("Unable to find the specified sound: " + name);
                return false;
            }
            return true;
        }

        private bool IsMusicValid(string name)
        {
            Sound s = Array.Find(musics, music => music.name == name);
            if (s == null)
            {
                UnityEngine.Debug.LogError("Unable to find the specified music: " + name);
                return false;
            }
            return true;
        }

        private IEnumerator SmoothOutSound_C(Sound s, float duration)
        {
            if (s != null)
            {
                s.isSmoothing = true;
                float currentVolume = s.currentVolume;
                float stride = (s.currentVolume * Time.fixedDeltaTime) / duration;
                while (currentVolume - stride < 0F)
                {
                    currentVolume -= stride;
                    s.source.volume = currentVolume;
                    stride = (s.currentVolume * Time.fixedDeltaTime) / duration;
                    yield return new WaitForFixedUpdate();
                }
                s.source.volume = s.currentVolume;
                s.source.Stop();
                s.isSmoothing = false;
            }
        }

        private IEnumerator SmoothInSound_C(Sound s, float duration)
        {
            if (s != null)
            {
                s.isSmoothing = true;
                float currentVolume = s.source.volume = 0f;
                s.source.Play();
                float stride = (s.currentVolume * Time.fixedDeltaTime) / duration;
                while (currentVolume + stride < s.currentVolume)
                {
                    currentVolume += stride;
                    s.source.volume = currentVolume;
                    stride = (s.currentVolume * Time.fixedDeltaTime) / duration;
                    yield return new WaitForFixedUpdate();
                }
                s.source.volume = s.currentVolume;
                s.isSmoothing = false;
            }
        }
    }
}