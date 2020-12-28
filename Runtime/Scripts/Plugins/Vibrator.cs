// Copyright (c) 2020 Matteo Beltrame

using UnityEngine;

namespace GibFrame.Plugins
{
    public class Vibrator
    {
        private AndroidJavaClass unityPlayer;
        private AndroidJavaObject activity;
        private AndroidJavaObject vibrator;
        private AndroidJavaClass vibrationEffectClass;

        private bool usesModenAPI = false;
        private int defaultAmplitude;

        public Vibrator()
        {
            if (IsOnAndroid())
            {
                unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                vibrator = activity.Call<AndroidJavaObject>("getSystemService", "vibrator");
                usesModenAPI = getSDKInt() >= 26;
                if (usesModenAPI)
                {
                    vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                    defaultAmplitude = vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE");
                }
            }
        }

        public void Cancel()
        {
            if (IsOnAndroid())
            {
                vibrator.Call("cancel");
            }
        }

        public bool HasAmplituideControl()
        {
            if (usesModenAPI)
            {
                return vibrator.Call<bool>("hasAmplitudeControl"); //API 26+ specific
            }
            else
            {
                return false; //If older than 26 then there is no amplitude control at all
            }
        }

        public void OneShot(long milliseconds, int amplitude = -1)
        {
            if (IsOnAndroid())
            {
                //If Android 8.0 (API 26+) or never use the new vibrationeffects
                if (usesModenAPI)
                {
                    amplitude = amplitude == -1 ? defaultAmplitude : amplitude;
                    ExecuteVibrationEffect("createOneShot", new object[] { milliseconds, amplitude });
                }
                else
                {
                    LegacyVibrate(milliseconds);
                }
            }
        }

        public void CreateWaveform(long[] timings, int repeat)
        {
            //Amplitude array varies between no vibration and default_vibration up to the number of timings

            if (IsOnAndroid())
            {
                //If Android 8.0 (API 26+) or never use the new vibrationeffects
                if (usesModenAPI)
                {
                    ExecuteVibrationEffect("createWaveform", new object[] { timings, repeat });
                }
                else
                {
                    LegacyVibrate(timings, repeat);
                }
            }
        }

        public void CreateWaveform(long[] timings, int[] amplitudes, int repeat)
        {
            if (IsOnAndroid())
            {
                //If Android 8.0 (API 26+) or never use the new vibrationeffects
                if (usesModenAPI)
                {
                    ExecuteVibrationEffect("createWaveform", new object[] { timings, amplitudes, repeat });
                }
                else
                {
                    LegacyVibrate(timings, repeat);
                }
            }
        }

        public bool HasVibrator()
        {
            return vibrator.Call<bool>("hasVibrator");
        }

        private void ExecuteVibrationEffect(string function, params object[] args)
        {
            AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>(function, args);
            vibrator.Call("vibrate", vibrationEffect);
        }

        private void LegacyVibrate(long milliseconds)
        {
            vibrator.Call("vibrate", milliseconds);
        }

        private void LegacyVibrate(long[] pattern, int repeat)
        {
            vibrator.Call("vibrate", pattern, repeat);
        }

        private bool IsOnAndroid()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
            return false;
#endif
        }

        private int getSDKInt()
        {
            if (IsOnAndroid())
            {
                using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
                {
                    return version.GetStatic<int>("SDK_INT");
                }
            }
            else
            {
                return -1;
            }
        }
    }
}