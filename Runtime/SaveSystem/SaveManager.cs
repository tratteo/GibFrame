using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;

namespace GibFrame.SaveSystem
{
    public static class SaveManager
    {
        public static string SaveJson<T>(T jsonClass, string path, bool pretty = true)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            var json = JsonUtility.ToJson(jsonClass, pretty);
            File.WriteAllText(path, json);
            return json;
        }

        public static bool TryLoadJson<T>(string path, out T jsonClass)
        {
            if (!File.Exists(path))
            {
                jsonClass = default;
                return false;
            }
            try
            {
                jsonClass = JsonUtility.FromJson<T>(File.ReadAllText(path));
                return true;
            }
            catch (Exception)
            {
                jsonClass = default;
                return false;
            }
        }

        /// <summary>
        ///   Save a generic type of data in the application persisten data path.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="data"> </param>
        /// <param name="path"> </param>
        /// <returns> SaveObject instance, null on error </returns>
        public static T SaveBinaryData<T>(T data, string path)
        {
            TryEncrypt(data);
            var formatter = new BinaryFormatter();
            var stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();
            return data;
        }

        /// <summary>
        ///   Load data from the specified path
        /// </summary>
        /// <param name="path"> </param>
        /// <returns>
        ///   A SaveObject instance. Use
        ///   <code>saveObject.GetData() </code>
        ///   to retrieve data. If the data is not present a null SaveObject will be returned
        /// </returns>
        public static bool TryLoadBinaryData<T>(string path, out T result)
        {
            if (File.Exists(path))
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open);
                if (stream.Length == 0)
                {
                    result = default;
                    return false;
                }

                var data = formatter.Deserialize(stream);
                if (data is EncryptedData)
                {
                    if ((data as EncryptedData).GetDeviceID() != SystemInfo.deviceUniqueIdentifier)
                    {
                        UnityEngine.Debug.LogError("Unauthorized to open encrypted file, identifiers not matching, aborting");
                        stream.Close();
                        result = default;
                        return false;
                    }
                }
                result = (T)data;
                stream.Close();
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }

        /// <summary>
        ///   Load the selected type of data. If not present, a newData object will be serialized at path and returned
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="newData"> </param>
        /// <param name="path"> </param>
        /// <returns> The retrieved or created data </returns>
        public static T LoadOrInitializeBinaryData<T>(T newData, string path)
        {
            if (!TryLoadBinaryData(path, out T data))
            {
                data = newData;
                SaveBinaryData(data, path);
            }

            return data;
        }

        /// <summary>
        ///   Delete data
        /// </summary>
        /// <param name="path"> </param>
        public static void DeleteObjectData(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        ///   Serialize a string to a file
        /// </summary>
        /// <param name="path"> </param>
        /// <param name="data"> </param>
        /// <param name="append"> </param>
        public static void SerializeToFile(string path, string data, bool append)
        {
            System.IO.Directory.CreateDirectory("data");
            if (append)
            {
                File.AppendAllText(path, data + Environment.NewLine);
            }
            else
            {
                File.WriteAllText(path, data + Environment.NewLine);
            }
        }

        private static void TryEncrypt<T>(T data)
        {
            if (data is EncryptedData)
            {
                (data as EncryptedData).Encrypt();
            }
        }

        public static class Async
        {
            private static Queue<SerializeJob> jobs;
            private static Thread serializeRoutine = null;

            public static void MarkDirty(params (object, string)[] elements)
            {
                jobs ??= new Queue<SerializeJob>();
                foreach (var elem in elements)
                {
                    jobs.Enqueue(new SerializeJob(elem.Item1, elem.Item2));
                }
                if (serializeRoutine == null || !serializeRoutine.IsAlive)
                {
                    serializeRoutine = new Thread(new ThreadStart(Routine));
                    serializeRoutine.Start();
                }
            }

            private static void Routine()
            {
                SerializeJob job;
                while (jobs.Count > 0)
                {
                    job = jobs.Dequeue();
                    SaveBinaryData(job.Data, job.Path);
                }
            }

            private class SerializeJob
            {
                public object Data { get; private set; }

                public string Path { get; private set; }

                public SerializeJob(object data, string path)
                {
                    Data = data;
                    Path = path;
                }
            }
        }
    }
}