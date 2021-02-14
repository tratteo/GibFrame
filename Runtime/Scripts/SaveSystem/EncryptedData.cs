// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.SaveSystem : EncryptedData.cs
//
// All Rights Reserved

using UnityEngine;

namespace GibFrame.SaveSystem
{
    [System.Serializable]
    public class EncryptedData
    {
        private string deviceId = null;
        private bool encrypted = false;

        internal string GetDeviceID()
        {
            return deviceId;
        }

        internal void Encrypt()
        {
            if (!encrypted)
            {
                deviceId = SystemInfo.deviceUniqueIdentifier;
                encrypted = true;
            }
        }
    }
}