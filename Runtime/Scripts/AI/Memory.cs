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
    public class Memory
    {
        private readonly List<MemoryModule> memories;
        private bool inhibit = false;

        public Memory()
        {
            memories = new List<MemoryModule>();
            Thread forgetThread = new Thread(() => Forget_T());
            forgetThread.Start();
        }

        ~Memory()
        {
            inhibit = true;
        }

        public void Remember(params MemoryModule[] modules)
        {
            foreach (MemoryModule module in modules)
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

        public bool Knows(object obj)
        {
            return memories.FindAll((m) => m.Memory.Equals(obj)).Count > 0;
        }

        public List<MemoryModule> GetAllMemories() => memories;

        public T[] GetAllMemories<T>(Predicate<MemoryModule> predicate, Converter<MemoryModule, T> mapper)
        {
            return memories.GetPredicatesMatchingObjects((m) => IsValidMemory(m) && predicate(m)).ConvertAll(mapper);
        }

        public MemoryModule Get(object value)
        {
            return memories.Find((m) => m.Memory.Equals(value) && IsValidMemory(m));
        }

        public bool Forget(object memory)
        {
            return ForgetAll((m) => m.Memory.Equals(memory)) > 0;
        }

        public int ForgetAll(Predicate<MemoryModule> Predicate)
        {
            return memories.RemoveAll(Predicate);
        }

        public bool Forget(MemoryModule module)
        {
            return memories.Remove(module);
        }

        public void Amnesia()
        {
            memories.Clear();
        }

        public bool IsValidMemory(MemoryModule module)
        {
            if (module.Memory is UnityEngine.Object)
            {
                return module.Memory as UnityEngine.Object;
            }
            return module.Memory != null;
        }

        private void Forget_T()
        {
            while (!inhibit)
            {
                foreach (MemoryModule module in memories)
                {
                    if (module != null)
                    {
                        module.TimeStep(0.05F);
                    }
                }
                ForgetAll((m) => m.RemembranceTime <= 0 && !IsValidMemory(m));
                Thread.Sleep(50);
            }
        }
    }
}