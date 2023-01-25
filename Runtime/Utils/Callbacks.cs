using System;

namespace GibFrame
{
    public abstract class AbstractCallback
    {
        public abstract void Invoke();
    }

    public class Callback<T> : AbstractCallback
    {
        private readonly Action<T> Event;
        private readonly T args;

        public Callback(Action<T> action, T args)
        {
            Event = action;
            this.args = args;
        }

        public override void Invoke()
        {
            Event?.Invoke(args);
        }
    }

    public class Callback<T, E> : AbstractCallback
    {
        private readonly T args0;
        private readonly E args1;

        private Action<T, E> Event;

        public Callback(Action<T, E> action, T args0, E args1)
        {
            Event = action;
            this.args0 = args0;
            this.args1 = args1;
        }

        public override void Invoke()
        {
            Event?.Invoke(args0, args1);
        }
    }

    public class Callback : AbstractCallback
    {
        private readonly Action Event;

        public Callback(Action action)
        {
            Event = action;
        }

        public override void Invoke()
        {
            Event?.Invoke();
        }
    }
}