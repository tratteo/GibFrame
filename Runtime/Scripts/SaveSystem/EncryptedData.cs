//Copyright (c) matteo
//EncryptedData.cs - com.tratteo.gibframe

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