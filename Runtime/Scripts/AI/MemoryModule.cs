//Copyright (c) matteo
//MemoryModule.cs - com.tratteo.gibframe

using System;

namespace GibFrame.AI
{
    public class MemoryModule : IEquatable<MemoryModule>
    {
        private float remembranceTime;

        public float RemembranceTime { get => remembranceTime; }

        public float InitialRemembranceTime { get; private set; }

        public object Memory { get; private set; }

        public MemoryModule(object memory, float remembranceTime)
        {
            this.remembranceTime = remembranceTime;
            InitialRemembranceTime = remembranceTime;
            Memory = memory;
        }

        public void Reset(float remembranceTime = -1)
        {
            this.remembranceTime = remembranceTime == -1 ? InitialRemembranceTime : remembranceTime;
        }

        public bool Equals(MemoryModule other)
        {
            return Memory.Equals(other.Memory);
        }

        public void TimeStep(float step)
        {
            remembranceTime -= step;
        }

        public override string ToString()
        {
            return Memory.ToString() + ", RT: " + InitialRemembranceTime;
        }
    }
}