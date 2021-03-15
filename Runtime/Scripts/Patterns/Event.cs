//Copyright (c) matteo
//Event.cs - com.tratteo.gibframe

using System;

namespace GibFrame.Patterns
{
    public class Event
    {
        private event Action Delegate;

        public Event()
        { }

        public void Subscribe(Action Callback)
        {
            Delegate += Callback;
        }

        public void Unsubscribe(Action Callback)
        {
            Delegate -= Callback;
        }

        public void Broadcast()
        {
            Delegate?.Invoke();
        }

        public void Purge()
        {
            Delegate[] delegates = Delegate.GetInvocationList();
            foreach (Delegate del in delegates)
            {
                Delegate -= (del as Action);
            }
            Delegate = null;
        }
    }

    public class Event<A>
    {
        private event Action<A> Delegate;

        public Event()
        { }

        public void Subscribe(Action<A> Callback)
        {
            Delegate += Callback;
        }

        public void Unsubscribe(Action<A> Callback)
        {
            Delegate -= Callback;
        }

        public void Broadcast(A arg)
        {
            Delegate?.Invoke(arg);
        }

        public void Purge()
        {
            Delegate[] delegates = Delegate.GetInvocationList();
            foreach (Delegate del in delegates)
            {
                Delegate -= (del as Action<A>);
            }
            Delegate = null;
        }
    }

    public class Event<A1, A2>
    {
        private event Action<A1, A2> Delegate;

        public Event()
        { }

        public void Subscribe(Action<A1, A2> Callback)
        {
            Delegate += Callback;
        }

        public void Unsubscribe(Action<A1, A2> Callback)
        {
            Delegate -= Callback;
        }

        public void Broadcast(A1 arg1, A2 arg2)
        {
            Delegate?.Invoke(arg1, arg2);
        }

        public void Purge()
        {
            Delegate[] delegates = Delegate.GetInvocationList();
            foreach (Delegate del in delegates)
            {
                Delegate -= (del as Action<A1, A2>);
            }
            Delegate = null;
        }
    }

    public class Event<A1, A2, A3>
    {
        private event Action<A1, A2, A3> Delegate;

        public Event()
        { }

        public void Subscribe(Action<A1, A2, A3> Callback)
        {
            Delegate += Callback;
        }

        public void Unsubscribe(Action<A1, A2, A3> Callback)
        {
            Delegate -= Callback;
        }

        public void Broadcast(A1 arg1, A2 arg2, A3 arg3)
        {
            Delegate?.Invoke(arg1, arg2, arg3);
        }

        public void Purge()
        {
            Delegate[] delegates = Delegate.GetInvocationList();
            foreach (Delegate del in delegates)
            {
                Delegate -= (del as Action<A1, A2, A3>);
            }
            Delegate = null;
        }
    }

    public class Event<A1, A2, A3, A4>
    {
        private event Action<A1, A2, A3, A4> Delegate;

        public Event()
        { }

        public void Subscribe(Action<A1, A2, A3, A4> Callback)
        {
            Delegate += Callback;
        }

        public void Unsubscribe(Action<A1, A2, A3, A4> Callback)
        {
            Delegate -= Callback;
        }

        public void Broadcast(A1 arg1, A2 arg2, A3 arg3, A4 arg4)
        {
            Delegate?.Invoke(arg1, arg2, arg3, arg4);
        }

        public void Purge()
        {
            Delegate[] delegates = Delegate.GetInvocationList();
            foreach (Delegate del in delegates)
            {
                Delegate -= (del as Action<A1, A2, A3, A4>);
            }
            Delegate = null;
        }
    }
}