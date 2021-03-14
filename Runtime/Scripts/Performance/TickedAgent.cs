//Copyright (c) matteo
//TickedAgent.cs - com.tratteo.gibframe

using System.Reflection;
using UnityEngine;

namespace GibFrame.Performance
{
    internal class TickedAgent
    {
        private float currentTick;

        public bool CustomDelta { get; private set; }

        public MethodInfo Method { get; private set; }

        public float TickDelta { get; set; }

        public bool TickDisabled { get; private set; }

        public MonoBehaviour Parent { get; private set; }

        public TickedAgent(MonoBehaviour parent, MethodInfo method, float tickDelta, bool tickDisabled, bool customDelta)
        {
            Method = method;
            Parent = parent;
            TickDelta = tickDelta;
            CustomDelta = customDelta;
            TickDisabled = tickDisabled;
            currentTick = 0;
        }

        public void Tick(float delta)
        {
            currentTick += delta;
            bool shouldTick;
            if (TickDisabled || (Parent.enabled && Parent.gameObject.activeSelf))
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
                if (currentTick >= TickDelta)
                {
                    Method?.Invoke(Parent, new object[] { TickDelta });
                    currentTick = 0;
                }
            }
        }
    }
}
