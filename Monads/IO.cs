using System;

namespace Monads
{
    // IO: () -> T
    public delegate T IO<out T>();

    public static partial class IO
    {
        // Create: T -> IO<T>
        public static IO<T> Create<T>(T value) => () => value;
        public static IO<T> Create<T>(Func<T> value) => () => value();
        public static IO<Unit> Create(Action action) => () =>
        {
            action();
            return Unit.Default;
        };
    }

    public static partial class IOExtensions
    {
        public static IO<TResult> SelectMany<TSource, TResult>(
            this IO<TSource> source,
            Func<TSource, IO<TResult>> selector) =>
                () =>
                {
                    TSource value = source();
                    return selector(value)();
                };

        // SelectMany: (IO<TSource>, TSource -> IO<TSelector>, (TSource, TSelector) -> TResult) -> IO<TResult>
        public static IO<TResult> SelectMany<TSource, TSelector, TResult>(
            this IO<TSource> source,
            Func<TSource, IO<TSelector>> selector,
            Func<TSource, TSelector, TResult> resultSelector) =>
                () =>
                {
                    TSource value = source();
                    return resultSelector(value, selector(value)());
                };

        // Wrap: TSource -> IO<TSource>
        public static IO<TSource> IO<TSource>(this TSource value) => () => value;

        // Select: (IO<TSource>, TSource -> TResult) -> IO<TResult>
        public static IO<TResult> Select<TSource, TResult>(
            this IO<TSource> source, Func<TSource, TResult> selector) =>
                source.SelectMany(value => selector(value).IO(), (value, result) => result);

        // Run: IO<T> -> T
        public static T Run<T>(this IO<T> io) => io();
    }
}
