// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.Performance : ICommonLateUpdate.cs
//
// All Rights Reserved

namespace GibFrame.Performance
{
    public interface ICommonLateUpdate
    {
        /// <summary>
        ///   Called by the <see cref="CommonUpdateRuntime"/> every LateUpdate
        /// </summary>
        /// <param name="deltaTime"> </param>
        void CommonLateUpdate(float deltaTime);
    }
}