using System;

namespace Monads
{
    // Cps: (T -> TContinuation>) -> TContinuation
    public delegate TContinuation Cps<TContinuation, out T>(Func<T, TContinuation> continuation);
    public static class Continuation
    {
        public static Cps<TContinuation, T> Return<T, TContinuation>(T value) =>
            continuation => continuation(value);
        public static Cps<TContinuation, T> Cps<TContinuation, T>(Func<T> function) =>
            continuation => continuation(function());
    }

    public static class ContinuationExtensions
    {
        // Wrap: TSource -> Cps<TContinuation, TSource>
        public static Cps<TContinuation, TSource> Cps<TContinuation, TSource>(
            this TSource value) =>
                continuation => continuation(value);

        public static Cps<TContinuation, TResult> Select<TSource, TContinuation, TResult>(
            this Cps<TContinuation, TSource> source,
            Func<TSource, TResult> selector) =>
                continuation => source(value => continuation(selector(value)));
        public static Cps<TContinuation, TResult> SelectMany<TSource, TContinuation, TResult>(
            this Cps<TContinuation, TSource> source,
            Func<TSource, Cps<TContinuation, TResult>> selector) =>
                continuation => source(value => selector(value)(continuation));

        // SelectMany: (Cps<TContinuation, TSource>, TSource -> Cps<TContinuation, TSelector>, (TSource, TSelector) -> TResult) -> Cps<TContinuation, TResult>
        public static Cps<TContinuation, TResult> SelectMany<TContinuation, TSource, TSelector, TResult>(
            this Cps<TContinuation, TSource> source,
            Func<TSource, Cps<TContinuation, TSelector>> selector,
            Func<TSource, TSelector, TResult> resultSelector) =>
                continuation => source(value =>
                    selector(value)(result =>
                        continuation(resultSelector(value, result))));
    }
}
