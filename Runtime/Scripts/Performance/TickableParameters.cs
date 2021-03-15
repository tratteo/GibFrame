namespace GibFrame.Performance
{
    public class TickableParameters
    {
        public float TickDelta { get; set; }

        public bool TickDisabled { get; private set; }

        public bool CustomDelta { get; private set; }

        public TickableParameters(float tickDelta, bool tickDisabled)
        {
            TickDelta = tickDelta;
            TickDisabled = tickDisabled;
            CustomDelta = tickDelta > 0F;
        }

        public TickableParameters(bool tickDisabled) : this(-1F, tickDisabled)
        {
        }

        public TickableParameters(float tickDelta) : this(tickDelta, false)
        {
        }

        public TickableParameters() : this(-1F, false)
        {
        }
    }
}
