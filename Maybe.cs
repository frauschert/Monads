using System;

namespace Monads
{
    public struct Maybe<T>
    {
        private readonly T value;

        private Maybe(T value)
        {
            this.value = value;
        }

        public static Maybe<T> Some(T value) => value;
        public static Maybe<T> None { get; } = new Maybe<T>();

        public static implicit operator Maybe<T>(T value) =>
            value == null ? None : new Maybe<T>(value);

        public TResult Match<TResult>(Func<T, TResult> some, Func<TResult> none) =>
            value == null ? none() : some(value);

        public override string ToString() =>
            Match(some: value => value.ToString(), () => "null");
    }
}
