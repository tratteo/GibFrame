// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.Utils : IProbSelectable.cs
//
// All Rights Reserved

namespace GibFrame.Utils
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