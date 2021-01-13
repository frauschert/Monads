using System;
using System.Collections.Generic;
using System.Text;

namespace Monads.Extensions
{
    public static partial class Try
    {
        public static Try<T> Catch<T>(this Try<T> source, Func<Exception, Try<T>> selector, Func<Exception, bool> predicate = null)
            => Catch<T, Exception>(source, selector, predicate);
        public static Try<T> Catch<T, TException>(this Try<T> source, Func<TException, Try<T>> selector, Func<TException, bool> predicate = null) where TException : Exception
            => source
                .Result
                .Match(_ => 
                    new Try<T>(() => source.Result), 
                    left => left is TException exc && (predicate == null || predicate(exc)) ? selector(exc) : new Try<T>(() => source.Result));

        public static TResult Finally<T, TResult>(this Try<T> source, Func<Try<T>, TResult> selector)
            => selector(source);
        public static void Finally<T>(this Try<T> source, Action<Try<T>> action)
            => action(source);

        public static Try<T> Throw<T>(this Exception exception)
            => new Try<T>(() => Either<Exception, T>.Left(exception));
    }
}
