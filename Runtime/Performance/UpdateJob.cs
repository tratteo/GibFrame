namespace GibFrame.Performance
{
    public class UpdateJob
    {
        private readonly AbstractCallback Job;
        private float currentTime = 0F;
        private float updateTime;

        public bool Active { get; private set; } = true;

        public UpdateJob(AbstractCallback Job, float updateTime, bool start = true)
        {
            this.Job = Job;
            this.updateTime = updateTime;
            Active = start;
        }

        public void EditUpdateTime(float updateTime)
        {
            this.updateTime = updateTime;
            currentTime = 0F;
        }

        public void Step(float deltaTime)
        {
            if (!Active) return;

            currentTime += deltaTime;
            if (ShouldExecute())
            {
                Job?.Invoke();
                currentTime = 0F;
            }
        }

        public void Suspend()
        {
            Active = false;
        }

        public void Resume()
        {
            Active = true;
            currentTime = 0F;
        }

        public float GetUpdateProgress() => currentTime / updateTime;

        protected bool ShouldExecute() => currentTime >= updateTime;
    }
}