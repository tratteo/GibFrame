using System;

namespace GibFrame
{
    /// <summary>
    ///   Wrapper class to decorate an object with a <see cref="System.Guid"/>
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public class GuidDecorator<T> : IEquatable<GuidDecorator<T>>
    {
        private readonly T payload;
        private readonly Guid guid;

        public Guid Guid => guid;

        public T Payload => payload;

        public GuidDecorator(T payload, Guid guid) : this(payload)
        {
            this.guid = guid;
        }

        public GuidDecorator(T payload)
        {
            this.payload = payload;
            guid = Guid.NewGuid();
        }

        public static implicit operator T(GuidDecorator<T> decorator) => decorator.Payload;

        public static implicit operator Guid(GuidDecorator<T> decorator) => decorator.Guid;

        public bool Equals(GuidDecorator<T> other) => other is not null && other.guid.Equals(guid);
    }
}