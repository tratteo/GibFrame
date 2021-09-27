// Copyright (c) Matteo Beltrame
//
// com.tratteo.gibframe -> GibFrame : Event.cs
//
// All Rights Reserved

using System;

namespace GibFrame
{
    public class Event
    {
        public event Action Invocation;

        public Event()
        { }

        public void Broadcast()
        {
            Invocation?.Invoke();
        }

        public void Purge()
        {
            if (Invocation != null)
            {
                Delegate[] delegates = Invocation.GetInvocationList();
                foreach (Delegate del in delegates)
                {
                    Invocation -= (del as Action);
                }
                Invocation = null;
            }
        }
    }

    public class Event<A>
    {
        public event Action<A> Invocation;

        public Event()
        { }

        public void Broadcast(A arg)
        {
            Invocation?.Invoke(arg);
        }

        public void Purge()
        {
            if (Invocation != null)
            {
                Delegate[] delegates = Invocation.GetInvocationList();
                foreach (Delegate del in delegates)
                {
                    Invocation -= (del as Action<A>);
                }
                Invocation = null;
            }
        }
    }

    public class Event<A1, A2>
    {
        public event Action<A1, A2> Invocation;

        public Event()
        { }

        public void Broadcast(A1 arg1, A2 arg2)
        {
            Invocation?.Invoke(arg1, arg2);
        }

        public void Purge()
        {
            if (Invocation != null)
            {
                Delegate[] delegates = Invocation.GetInvocationList();
                foreach (Delegate del in delegates)
                {
                    Invocation -= (del as Action<A1, A2>);
                }
                Invocation = null;
            }
        }
    }

    public class Event<A1, A2, A3>
    {
        public event Action<A1, A2, A3> Invocation;

        public Event()
        { }

        public void Broadcast(A1 arg1, A2 arg2, A3 arg3)
        {
            Invocation?.Invoke(arg1, arg2, arg3);
        }

        public void Purge()
        {
            if (Invocation != null)
            {
                Delegate[] delegates = Invocation.GetInvocationList();
                foreach (Delegate del in delegates)
                {
                    Invocation -= (del as Action<A1, A2, A3>);
                }
                Invocation = null;
            }
        }
    }

    public class Event<A1, A2, A3, A4>
    {
        public event Action<A1, A2, A3, A4> Invocation;

        public Event()
        { }

        public void Broadcast(A1 arg1, A2 arg2, A3 arg3, A4 arg4)
        {
            Invocation?.Invoke(arg1, arg2, arg3, arg4);
        }

        public void Purge()
        {
            if (Invocation != null)
            {
                Delegate[] delegates = Invocation.GetInvocationList();
                foreach (Delegate del in delegates)
                {
                    Invocation -= (del as Action<A1, A2, A3, A4>);
                }
                Invocation = null;
            }
        }
    }
}