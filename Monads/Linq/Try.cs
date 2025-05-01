using Monads.Extensions;
using System;

namespace Monads.Linq
{
    public static class Try
    {
        public static Try<TResult> Select<T, TResult>(this Try<T> @try, Func<T, TResult> selector) =>
            SelectMany(@try, value => new Try<TResult>(() => selector(value)));
        public static Try<TResult> SelectMany<T, TResult>(this Try<T> @try, Func<T, Try<TResult>> selector) =>
            @try.Result.Match(selector, e => new Try<TResult>(() => throw e));

        public static Try<TResult> SelectMany<T, T2, TResult>(this Try<T> @try, Func<T, Try<T2>> selector, Func<T, T2, TResult> resultSelector) =>
            @try.Result.Match(right => selector(right).Result.Match(right2 => new Try<TResult>(() => resultSelector(right, right2)), left2 => left2.Throw<TResult>()), left => left.Throw<TResult>()); 
    }
}
