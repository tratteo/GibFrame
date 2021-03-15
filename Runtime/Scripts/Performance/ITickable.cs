namespace GibFrame.Performance
{
    public interface ITickable
    {
        void Tick(float tickDelta);

        TickableParameters GetParameters();
    }
}
