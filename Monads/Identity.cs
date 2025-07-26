using System;

namespace Monads
{
    // Identity monad: wraps a value and provides monadic operations.
    public readonly struct Identity<T>
    {
        public T Value { get; }

        public Identity(T value) => Value = value;

        // Functor: Map/Select
        public Identity<TResult> Select<TResult>(Func<T, TResult> selector) =>
            new Identity<TResult>(selector(Value));

        // Monad: Bind/SelectMany
        public Identity<TResult> SelectMany<TResult>(Func<T, Identity<TResult>> binder) =>
            binder(Value);

        // Monad: Bind/SelectMany with projection (LINQ support)
        public Identity<TResult> SelectMany<TBind, TResult>(
            Func<T, Identity<TBind>> binder,
            Func<T, TBind, TResult> projector) =>
                new Identity<TResult>(projector(Value, binder(Value).Value));
    }

    public static class Identity
    {
        // Helper to create an Identity monad
        public static Identity<T> Create<T>(T value) => new Identity<T>(value);
    }
}
