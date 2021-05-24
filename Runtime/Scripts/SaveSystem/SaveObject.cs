// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.SaveSystem : SaveObject.cs
//
// All Rights Reserved

namespace GibFrame.SaveSystem
{
    public class SaveObject
    {
        private object data;

        public SaveObject(object data)
        {
            this.data = data;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns> The T casted type of data </returns>
        public T GetData<T>()
        {
            return (T)data;
        }
    }
}