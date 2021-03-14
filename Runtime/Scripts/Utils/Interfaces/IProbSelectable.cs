//Copyright (c) matteo
//IProbSelectable.cs - com.tratteo.gibframe

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