//Copyright (c) matteo
//SaveManager.cs - com.tratteo.gibframe

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
        //Can write here the static file names to use in the game
        //I.E
        //public static readonly string PLAYER_DATA = Application.persistentDataPath + "/player_data.data";
        //Calling the methods will look like this:
        //SaveManager.GetInstance().SavePersistentData<T>(data, SaveManager.PLAYER_DATA);

        /// <summary>
        ///   Save a generic type of data in the application persisten data path.
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="data"> </param>
        /// <param name="path"> </param>
        /// <returns> SaveObject instance, null on error </returns>
        public static SaveObject SavePersistentData<T>(T data, string path)
        {
            TryEncrypt(data);
            SaveObject saveObject = new SaveObject(data);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();
            return saveObject;
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
        public static SaveObject LoadPersistentData(string path)
        {
            SaveObject saveObject;
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                if (stream.Length == 0)
                    return null;
                object data = formatter.Deserialize(stream);
                if (data is EncryptedData)
                {
                    if ((data as EncryptedData).GetDeviceID() != SystemInfo.deviceUniqueIdentifier)
                    {
                        Debug.LogError("Unauthorized to open encrypted file, identifiers not matching, aborting");
                        stream.Close();
                        return null;
                    }
                }
                saveObject = new SaveObject(data);
                stream.Close();
                return saveObject;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///   Load the selected type of data. If not present, a newData object will be serialized at path and returned
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="newData"> </param>
        /// <param name="path"> </param>
        /// <returns> The retrieved or created data </returns>
        public static T LoadOrInitialize<T>(T newData, string path)
        {
            T data;
            SaveObject saveObject = LoadPersistentData(path);
            if (saveObject != null)
            {
                data = saveObject.GetData<T>();
            }
            else
            {
                data = newData;
                SavePersistentData(data, path);
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
                jobs = jobs ?? new Queue<SerializeJob>();
                foreach ((object, string) elem in elements)
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
                    SavePersistentData(job.Data, job.Path);
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