// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.AI : MemoryModule.cs
//
// All Rights Reserved

using System;

namespace GibFrame.AI
{
    public class MemoryModule : IEquatable<MemoryModule>
    {
        private float remembranceTime;

        public float RemembranceTime => remembranceTime;

        public float InitialRemembranceTime { get; private set; }

        public object Memory { get; private set; }

        public MemoryModule(object memory, float remembranceTime)
        {
            this.remembranceTime = remembranceTime;
            InitialRemembranceTime = remembranceTime;
            Memory = memory;
        }

        public override string ToString()
        {
            return Memory.ToString() + ", Initial remembrance time: " + InitialRemembranceTime;
        }

        public void Reset(float remembranceTime = -1) => this.remembranceTime = remembranceTime == -1 ? InitialRemembranceTime : remembranceTime;

        public void TimeStep(float step) => remembranceTime -= step;

        public bool Equals(MemoryModule other) => Memory.Equals(other.Memory);
    }
}