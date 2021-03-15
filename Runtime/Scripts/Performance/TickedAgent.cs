//Copyright (c) matteo
//TickedAgent.cs - com.tratteo.gibframe

using UnityEngine;

namespace GibFrame.Performance
{
    internal class TickedAgent
    {
        private float currentTick;

        private ITickable agent;

        public TickableParameters Parameters { get; private set; }

        public MonoBehaviour Parent { get; private set; }

        public TickedAgent(ITickable tickable, float defalutTickDelta)
        {
            agent = tickable;
            Parameters = tickable.GetParameters();
            Parent = tickable as MonoBehaviour;
            if (!Parameters.CustomDelta)
            {
                Parameters.TickDelta = defalutTickDelta;
            }
        }

        public void Reset()
        {
            currentTick = 0;
        }

        public bool CanTick() => currentTick >= Parameters.TickDelta;

        public void Step(float increment)
        {
            currentTick += increment;
        }

        public void Tick()
        {
            bool shouldTick;
            if (Parameters.TickDisabled || (Parent.enabled && Parent.gameObject.activeSelf))
            {
                shouldTick = true;
            }
            else
            {
                shouldTick = false;
                currentTick = 0;
            }

            if (shouldTick)
            {
                agent.Tick(Parameters.TickDelta);
                currentTick = 0;
            }
        }
    }
}
