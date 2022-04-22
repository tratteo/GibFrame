// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame.AI : Memory.cs
//
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Threading;

namespace GibFrame.AI
{
    public class Memory : IDisposable
    {
        private readonly List<MemoryModule> memories;
        private readonly float processDeltaTime = 0.1F;
        private bool inhibit = false;
        private Thread processThread;

        public Memory(float processDeltaTime)
        {
            memories = new List<MemoryModule>();
            this.processDeltaTime = processDeltaTime;
        }

        public void StartProcessing()
        {
            if (processThread != null)
            {
                inhibit = false;
                processThread = new Thread(() => Forget_T());
                processThread.Start();
            }
        }

        public void StopProcessing()
        {
            if (processThread != null)
            {
                inhibit = true;
                processThread.Abort();
            }
        }

        public void Remembers(params MemoryModule[] modules)
        {
            foreach (var module in modules)
            {
                MemoryModule mod;
                if ((mod = Get(module.Memory)) != null)
                {
                    mod.Reset();
                }
                else
                {
                    memories.Add(module);
                }
            }
        }

        public bool Knows(object obj) => memories.FindAll((m) => m.Memory.Equals(obj)).Count > 0;

        public List<MemoryModule> GetAllMemories() => memories;

        public IReadOnlyCollection<T> GetAllMemories<T>(Predicate<MemoryModule> predicate, Converter<MemoryModule, T> converter) => memories.FindAll((m) => IsValidMemory(m) && predicate(m)).ConvertAll(converter);

        public MemoryModule Get(object value) => memories.Find((m) => m.Memory.Equals(value) && IsValidMemory(m));

        public bool Forget(object memory) => ForgetAll((m) => m.Memory.Equals(memory)) > 0;

        public int ForgetAll(Predicate<MemoryModule> Predicate) => memories.RemoveAll(Predicate);

        public bool Forget(MemoryModule module) => memories.Remove(module);

        public void Amnesia() => memories.Clear();

        public bool IsValidMemory(MemoryModule module)
        {
            if (module.Memory is UnityEngine.Object)
            {
                return module.Memory as UnityEngine.Object;
            }
            return module.Memory != null;
        }

        public void Dispose()
        {
            inhibit = true;
            processThread?.Abort();
        }

        private void Forget_T()
        {
            var millis = (int)(processDeltaTime * 1000);
            while (!inhibit)
            {
                foreach (var module in memories)
                {
                    if (module != null)
                    {
                        module.TimeStep(processDeltaTime);
                    }
                }
                ForgetAll((m) => m.RemembranceTime <= 0 && !IsValidMemory(m));
                Thread.Sleep(millis);
            }
        }
    }
}
