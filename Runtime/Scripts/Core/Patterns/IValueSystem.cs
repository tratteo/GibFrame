using System;

namespace GibFrame
{
    public interface IValueSystem
    {
        public float Value { get; }

        public float MaxValue { get; }

        public float ValuePercentage { get; }

        public event Action<float, float> Changed;
    }
}
