// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.Performance : ICommonFixedUpdate.cs
//
// All Rights Reserved

namespace GibFrame.Performance
{
    public interface ICommonFixedUpdate
    {
        /// <summary>
        ///   Called by the <see cref="CommonUpdateRuntime"/> every FixedUpdate
        /// </summary>
        /// <param name="deltaTime"> </param>
        void CommonFixedUpdate(float fixedDeltaTime);
    }
}