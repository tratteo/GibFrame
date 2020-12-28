// Copyright (c) 2020 Matteo Beltrame

using GibFrame.Patterns;
using System;
using System.Collections;
using UnityEngine;

namespace GibFrame.Audio
{
    public class AudioManager : MonoSingleton<AudioManager>
    {
        [SerializeField] private Sound[] sounds = null;
        [SerializeField] private Sound[] musics = null;

        private bool audioActive = true;
        private bool musicActive = true;

        private Sound currentMusic = null;

        /// <summary>
        ///   <para> Smooth out a sound in a specified time with specified stride chunks </para>
        ///   Returns: the smoothed sound
        /// </summary>
        public void SmoothOutSound(string name, float duration)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s.source.isPlaying)
            {
                StartCoroutine(SmoothOutSound_C(s, duration));
            }
        }

        /// <summary>
        ///   Smooth in a sound in a specified time with specified stride chunks
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="stride"> </param>
        /// <param name="duration"> </param>
        /// <returns> The sound smoothed in </returns>
        public void SmoothInSound(string name, float duration)
        {
            if (!audioActive)
            {
                return;
            }

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (!s.source.isPlaying)
            {
                StartCoroutine(SmoothInSound_C(s, duration));
            }
        }

        /// <summary>
        ///   <para> Smooth out a sound in a specified time with specified stride chunks </para>
        ///   Returns: the smoothed sound
        /// </summary>
        public void SmoothOutMusic(string name, float duration)
        {
            Sound s = Array.Find(musics, sound => sound.name == name);
            if (s.source.isPlaying)
            {
                StartCoroutine(SmoothOutSound_C(s, duration));
                currentMusic = null;
            }
        }

        /// <summary>
        ///   Smooth in a sound in a specified time with specified stride chunks
        /// </summary>
        /// <param name="name"> </param>
        /// <param name="stride"> </param>
        /// <param name="duration"> </param>
        /// <returns> The sound smoothed in </returns>
        public void SmoothInMusic(string name, float duration)
        {
            if (!audioActive)
            {
                return;
            }

            Sound s = Array.Find(musics, sound => sound.name == name);
            if (!s.source.isPlaying)
            {
                StartCoroutine(SmoothInSound_C(s, duration));
                currentMusic = s;
            }
        }

        /// <summary>
        ///   Play a music
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> </returns>
        public void PlayMusic(string name)
        {
            if (!musicActive)
            {
                return;
            }

            Sound s = Array.Find(musics, sound => sound.name == name);
            if (!s.source.isPlaying)
            {
                s.source.Play();
            }
            currentMusic = s;
        }

        /// <summary>
        ///   Stop the currently playing music
        /// </summary>
        public void StopMusic()
        {
            if (currentMusic != null)
            {
                currentMusic.source.Stop();
                currentMusic = null;
            }
        }

        /// <summary>
        ///   Play a sound
        /// </summary>
        /// <param name="name"> </param>
        /// <returns> Sound instance </returns>
        public void PlaySound(string name)
        {
            if (!audioActive)
            {
                return;
            }

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (!s.source.isPlaying)
            {
                s.source.Play();
            }
        }

        /// <summary>
        ///   Stop a sound specified by its name
        /// </summary>
        /// <param name="name"> </param>
        public void StopSound(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (!s.source.isPlaying)
            {
                s.source.Stop();
            }
        }

        /// <summary>
        ///   Edit the sounds volume
        /// </summary>
        /// <param name="volume"> </param>
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

        /// <summary>
        ///   Edit the music volume
        /// </summary>
        /// <param name="volume"> </param>
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

        /// <summary>
        ///   Toggle the sounds
        /// </summary>
        /// <param name="audioActive"> </param>
        public void ToggleSounds(bool audioActive)
        {
            if (!audioActive)
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

            this.audioActive = audioActive;
        }

        /// <summary>
        ///   Toggle the music
        /// </summary>
        /// <param name="musicActive"> </param>
        public void ToggleMusic(bool musicActive)
        {
            if (!musicActive)
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

            this.musicActive = musicActive;
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