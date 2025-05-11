using System;
using System.Collections.Generic;
using System.Linq;

namespace Monads
{
    public readonly struct Maybe<T>
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

        public static partial class Maybe
    {
        public static Maybe<T> SingleOrNone<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            try
            {
                return enumerable.Single(predicate);
            }
            catch (Exception)
            {
                return Maybe<T>.None;
            }
        }

        public static Maybe<int> TryParseInt(this string str) =>
            int.TryParse(str, out var result) ? result : Maybe<int>.None;

        public static Maybe<TValue> TryGetValue<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TKey key) =>
                dictionary.TryGetValue(key, out var value) ? value : Maybe<TValue>.None;

        public static IEnumerable<T> ToEnumerable<T>(this Maybe<T> maybe) =>
            maybe.Match(value => Enumerable.Repeat(value, 1), () => Enumerable.Empty<T>());
    }
}
