// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : IProbSelectable.cs
//
// All Rights Reserved

namespace GibFrame
{
    /// <summary>
    ///   Implement this interface to make an object selectable based on a certain probability
    /// </summary>
    public interface IProbSelectable
    {
        float ProvideSelectProbability();

        void SetSelectProbability(float prob);
    }
}