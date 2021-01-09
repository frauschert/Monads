using System;
using System.Collections.Generic;
using System.Text;

namespace Monads.Linq
{
    public static class Try
    {
        public static Try<TResult> Select<T, TResult>(this Try<T> @try, Func<T, TResult> selector) =>
            SelectMany<T, TResult>(@try, value => () => selector(value));
        public static Try<TResult> SelectMany<T, TResult>(this Try<T> @try, Func<T, Try<TResult>> selector) =>
            @try.Match(selector, e => new Try<TResult>();

        public static Try<TResult> SelectMany<T, T2, TResult>(this Try<T> @try, Func<T, Try<T2>> selector, Func<T, T2, TResult> resultSelector) =>
            @try.Match(t => selector(t).Match(t2 => new Try<TResult>(() => resultSelector(t, t2)), e => () => Either<Exception, TResult>.Left(e)), e => () => Either<Exception, TResult>.Left(e));
    }
}
