// Copyright (c) Matteo Beltrame
//
// Package com.tratteo.gibframe : GibFrame.Utils.Callbacks : Callbacks.cs
//
// All Rights Reserved

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GibFrame.Utils.Callbacks
{
    public abstract class AbstractCallback
    {
        public abstract void Invoke();
    }

    public class Callback<T> : AbstractCallback
    {
        private T args;

        public Callback(Action<T> action, T args)
        {
            this.Event = action;
            this.args = args;
        }

        private event Action<T> Event;

        public override void Invoke()
        {
            Event?.Invoke(args);
        }
    }

    public class Callback<T, E> : AbstractCallback
    {
        private T args0;
        private E args1;

        public Callback(Action<T, E> action, T args0, E args1)
        {
            this.Event = action;
            this.args0 = args0;
            this.args1 = args1;
        }

        private event Action<T, E> Event;

        public override void Invoke()
        {
            Event?.Invoke(args0, args1);
        }
    }

    public class Callback : AbstractCallback
    {
        public Callback(Action action)
        {
            this.Event = action;
        }

        private event Action Event;

        public override void Invoke()
        {
            Event?.Invoke();
        }
    }
}