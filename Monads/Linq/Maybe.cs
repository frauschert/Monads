using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace Monads.Linq
{
    public static partial class Maybe
    {
        public static Maybe<TResult> Select<T, TResult>(this Maybe<T> maybe, Func<T, TResult> selector) =>
            SelectMany<T, TResult>(maybe, value => selector(value));

        public static Maybe<TResult> SelectMany<T, TResult>(this Maybe<T> maybe, Func<T, Maybe<TResult>> selector) =>
            maybe.Match(value => selector(value), () => Maybe<TResult>.None);

        public static Maybe<TResult> SelectMany<T, T2, TResult>(this Maybe<T> maybe, Func<T, Maybe<T2>> selector, Func<T, T2, TResult> resultSelector) =>
            maybe.Match(t => selector(t).Match(t2 => resultSelector(t, t2), () => Maybe<TResult>.None), () => Maybe<TResult>.None);

        public static Maybe<T> Where<T>(this Maybe<T> maybe, Func<T, bool> predicate) =>
            maybe.Match(value => predicate(value) ? value : Maybe<T>.None, () => Maybe<T>.None);

        public static Maybe<TResult> Combine<T, T2, TResult>(this Maybe<T> first, Maybe<T2> second, Func<T, T2, TResult> resultSelector) =>
            SelectMany(first, _ => second, resultSelector);
    }
}
