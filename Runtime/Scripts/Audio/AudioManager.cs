//Copyright (c) matteo
//AudioManager.cs - com.tratteo.gibframe

using System;
using System.Collections;
using GibFrame.Patterns;
using UnityEngine;

namespace GibFrame.Audio
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] private Sound[] sounds = null;
        [SerializeField] private Sound[] musics = null;

        public bool SoundActive { get; private set; } = true;

        public bool MusicActive { get; private set; } = true;

        public void PlaySound(string name)
        {
            if (!SoundActive)
            {
                return;
            }

            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.Play();
        }

        public void StopSound(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.Stop();
        }

        public bool IsSoundPlaying(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            return s != null && s.source.isPlaying;
        }

        public void SmoothOutSound(string name, float duration)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s.source.isPlaying)
            {
                StartCoroutine(SmoothOutSound_C(s, duration));
            }
        }

        public void SmoothInSound(string name, float duration)
        {
            if (!SoundActive)
            {
                return;
            }

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

        public void ToggleSounds(bool active)
        {
            if (!active)
            {
                int length = sounds.Length;
                for (int i = 0; i < length; i++)
                {
                    if (sounds[i].source.isPlaying)
                    {
                        sounds[i].source.volume = 0F;
                    }
                }
            }

            SoundActive = active;
        }

        public void PlayMusic(string name)
        {
            if (!MusicActive)
            {
                return;
            }

            Sound s = Array.Find(musics, sound => sound.name == name);
            s.source.Play();
        }

        public void StopMusic(string name)
        {
            Sound s = Array.Find(musics, sound => sound.name == name);
            s.source.Stop();
        }

        public bool IsMusicPlaying(string name)
        {
            Sound s = Array.Find(musics, music => music.name == name);
            return s != null && s.source.isPlaying;
        }

        public void SmoothOutMusic(string name, float duration)
        {
            Sound s = Array.Find(musics, sound => sound.name == name);
            if (s.source.isPlaying)
            {
                StartCoroutine(SmoothOutSound_C(s, duration));
            }
        }

        public void SmoothInMusic(string name, float duration)
        {
            if (!MusicActive)
            {
                return;
            }

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

        public void ToggleMusic(bool active)
        {
            if (!active)
            {
                int length = musics.Length;
                for (int i = 0; i < length; i++)
                {
                    if (musics[i].source.isPlaying)
                    {
                        musics[i].source.volume = 0F;
                    }
                }
            }

            MusicActive = active;
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