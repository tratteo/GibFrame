using GibFrame.Utils.Callbacks;

namespace GibFrame.Patterns
{
    public class UpdateJob
    {
        private float currentTime = 0F;
        private float updateTime;
        private AbstractCallback Job;

        public UpdateJob(AbstractCallback Job, float updateTime)
        {
            this.Job = Job;
            this.updateTime = updateTime;
        }

        public void Step(float deltaTime)
        {
            currentTime += deltaTime;
            if (ShouldExecute())
            {
                Job?.Invoke();
                currentTime = 0F;
            }
        }

        public float GetUpdateProgress() => currentTime / updateTime;

        protected bool ShouldExecute() => currentTime >= updateTime;
    }
}
