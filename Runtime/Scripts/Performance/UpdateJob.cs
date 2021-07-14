// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.Performance : UpdateJob.cs
//
// All Rights Reserved

namespace GibFrame.Performance
{
    public class UpdateJob : ICommonUpdate
    {
        private readonly AbstractCallback Job;
        private float currentTime = 0F;
        private float updateTime;

        public bool Active { get; private set; } = true;

        public UpdateJob(AbstractCallback Job, float updateTime, bool start = true)
        {
            this.Job = Job;
            this.updateTime = updateTime;
            if (start)
            {
                CommonUpdateManager.Register(this);
            }
        }

        public void EditUpdateTime(float updateTime)
        {
            this.updateTime = updateTime;
            currentTime = 0F;
        }

        public void CommonUpdate(float deltaTime)
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
            CommonUpdateManager.Unregister(this);
            Active = false;
        }

        public void Resume()
        {
            CommonUpdateManager.Register(this);
            Active = true;
            currentTime = 0F;
        }

        public float GetUpdateProgress() => currentTime / updateTime;

        protected bool ShouldExecute() => currentTime >= updateTime;
    }
}
