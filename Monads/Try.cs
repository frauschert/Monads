using System;

namespace Monads
{
    public sealed class Try<T>
    {
        private readonly Lazy<Either<Exception, T>> factory;
        public Try(Func<T> factory) => 
            this.factory = new Lazy<Either<Exception, T>>(() =>
            {
                try
                {
                    return factory();
                }
                catch (Exception ex)
                {
                    return ex;
                }
            });

        public bool HasException => 
            factory.Value.Match(_ => false, _ => true);

        public TResult Match<TResult>(
            Func<T, TResult> right,
            Func<Exception, TResult> left) =>
                factory.Value.Match(right, left);


        public static Try<T> Create(Func<T> factory) =>
            new Try<T>(factory);

        public static Try<T> Create(IO<T> factory) =>
            new Try<T>(() => factory());

        public static Try<Unit> Create(Action factory) =>
            new Try<Unit>(() =>
            {
                factory();
                return Unit.Default;
            });

        public static implicit operator Try<T>(T value) => new Try<T>(() => value);
        public static implicit operator Try<T>(Func<T> factory) => 
            new Try<T>(factory);

        public static implicit operator Try<T>(IO<T> factory) =>
            new Try<T>(() => factory());
    }

    public static partial class TryExtensions
    {
        public static Try<T> Catch<T>(
            this Try<T> source, 
            Func<Exception, Try<T>> selector, 
            Func<Exception, bool> predicate = null) => 
                Catch<T, Exception>(source, selector, predicate);

        public static Try<T> Catch<T, TException>(
            this Try<T> source, 
            Func<TException, Try<T>> selector, 
            Func<TException, bool> predicate = null) where TException : Exception => 
                source.Match(
                    _ => source, 
                    left => left is TException exc && (predicate == null || predicate(exc)) ? selector(exc) : source);

        public static Try<T> Catch<T>(
            this Try<T> source,
            Action<Exception> action,
            Func<Exception, bool> predicate = null) =>
                Catch<T, Exception>(source, action, predicate);

        public static Try<T> Catch<T, TException>(
            this Try<T> source,
            Action<TException> action,
            Func<TException, bool> predicate = null) =>
                source.Match(
                    _ => source, 
                    left =>
                    {
                        if (left is TException exc && (predicate == null || predicate(exc)))
                        {
                            action(exc);
                        }
                        return source;
                    });

        public static TResult Finally<T, TResult>(
            this Try<T> source, 
            Func<Try<T>, TResult> selector) => 
                selector(source);

        public static void Finally<T>(
            this Try<T> source, 
            Action<Try<T>> action) => 
                action(source);

        public static Try<T> Throw<T>(
            this Exception exception) => 
                new Try<T>(() => throw exception);

        public static Try<TResult> Select<T, TResult>(
            this Try<T> source,
            Func<T, TResult> selector) =>
                SelectMany(source, value => 
                    new Try<TResult>(() => 
                        selector(value)));

        public static Try<TResult> SelectMany<T, TResult>(
            this Try<T> source,
            Func<T, Try<TResult>> selector) =>
                source.Match(selector, e => e.Throw<TResult>());

        public static Try<TResult> SelectMany<T, T2, TResult>(
            this Try<T> source,
            Func<T, Try<T2>> selector,
            Func<T, T2, TResult> resultSelector) =>
                source.Match(right =>
                    selector(right).Match(right2 =>
                        new Try<TResult>(() => resultSelector(right, right2)),
                        left2 => left2.Throw<TResult>()),
                    left => left.Throw<TResult>());
    }
}
