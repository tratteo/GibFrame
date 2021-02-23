// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : com.tratteo.gibframe.Packages.com.tratteo.gibframe.Runtime.Scripts.Patterns : Event.cs
//
// All Rights Reserved

using System;

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

public class Event<T>
{
    private event Action<T> Delegate;

    public Event()
    { }

    public void Subscribe(Action<T> Callback)
    {
        Delegate += Callback;
    }

    public void Unsubscribe(Action<T> Callback)
    {
        Delegate -= Callback;
    }

    public void Broadcast(T arg)
    {
        Delegate?.Invoke(arg);
    }

    public void Purge()
    {
        Delegate[] delegates = Delegate.GetInvocationList();
        foreach (Delegate del in delegates)
        {
            Delegate -= (del as Action<T>);
        }
        Delegate = null;
    }
}

public class Event<T1, T2>
{
    private event Action<T1, T2> Delegate;

    public Event()
    { }

    public void Subscribe(Action<T1, T2> Callback)
    {
        Delegate += Callback;
    }

    public void Unsubscribe(Action<T1, T2> Callback)
    {
        Delegate -= Callback;
    }

    public void Broadcast(T1 arg1, T2 arg2)
    {
        Delegate?.Invoke(arg1, arg2);
    }

    public void Purge()
    {
        Delegate[] delegates = Delegate.GetInvocationList();
        foreach (Delegate del in delegates)
        {
            Delegate -= (del as Action<T1, T2>);
        }
        Delegate = null;
    }
}

public class Event<T1, T2, T3>
{
    private event Action<T1, T2, T3> Delegate;

    public Event()
    { }

    public void Subscribe(Action<T1, T2, T3> Callback)
    {
        Delegate += Callback;
    }

    public void Unsubscribe(Action<T1, T2, T3> Callback)
    {
        Delegate -= Callback;
    }

    public void Broadcast(T1 arg1, T2 arg2, T3 arg3)
    {
        Delegate?.Invoke(arg1, arg2, arg3);
    }

    public void Purge()
    {
        Delegate[] delegates = Delegate.GetInvocationList();
        foreach (Delegate del in delegates)
        {
            Delegate -= (del as Action<T1, T2, T3>);
        }
        Delegate = null;
    }
}

public class Event<T1, T2, T3, T4>
{
    private event Action<T1, T2, T3, T4> Delegate;

    public Event()
    { }

    public void Subscribe(Action<T1, T2, T3, T4> Callback)
    {
        Delegate += Callback;
    }

    public void Unsubscribe(Action<T1, T2, T3, T4> Callback)
    {
        Delegate -= Callback;
    }

    public void Broadcast(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        Delegate?.Invoke(arg1, arg2, arg3, arg4);
    }

    public void Purge()
    {
        Delegate[] delegates = Delegate.GetInvocationList();
        foreach (Delegate del in delegates)
        {
            Delegate -= (del as Action<T1, T2, T3, T4>);
        }
        Delegate = null;
    }
}
