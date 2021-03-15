//Copyright (c) matteo
//TickedAttribute.cs - com.tratteo.gibframe

using System;

namespace GibFrame.Performance
{
    public class TickedAttribute : Attribute
    {
        public float TickDelta { get; private set; }

        public bool TickDisabled { get; private set; }

        public TickedAttribute()
        {
            TickDelta = -1F;
            TickDisabled = false;
        }

        public TickedAttribute(float tickDelta) : this(tickDelta, false)
        {
        }

        public TickedAttribute(bool tickDisabled) : this(-1F, tickDisabled)
        {
        }

        public TickedAttribute(float tickDelta, bool tickDisabled)
        {
            TickDelta = tickDelta;
            TickDisabled = tickDisabled;
        }
    }
}