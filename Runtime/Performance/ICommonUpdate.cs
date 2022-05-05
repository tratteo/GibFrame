// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.Performance : ICommonUpdate.cs
//
// All Rights Reserved

namespace GibFrame.Performance
{
    public interface ICommonUpdate
    {
        /// <summary>
        ///   Called by the <see cref="CommonUpdateRuntime"/> every Update
        /// </summary>
        /// <param name="deltaTime"> </param>
        void CommonUpdate(float deltaTime);
    }
}