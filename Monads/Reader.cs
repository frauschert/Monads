using System;

namespace Monads
{
    // Reader: TEnvironment -> T
    public delegate T Reader<in TEnvironment, out T>(TEnvironment environment);
    public static partial class Reader
    {
        // Create: T -> Reader<TEnvironment, T>
        public static Reader<TEnvironment, T> Create<TEnvironment, T>(T value) =>
            environment => value;
        // Create: Func<TEnvironment, T> -> Reader<TEnvironment, T>
        public static Reader<TEnvironment, T> Create<TEnvironment, T>(Func<TEnvironment, T> func) =>
            environment => func(environment);
    }

    public static partial class ReaderExtensions
    {
        public static Reader<TEnvironment, TResult> SelectMany<TEnvironment, TSource, TResult>(
            this Reader<TEnvironment, TSource> source,
            Func<TSource, Reader<TEnvironment, TResult>> selector) =>
                environment =>
                {
                    TSource value = source(environment);
                    return selector(value)(environment);
                };

        // SelectMany: (Reader<TEnvironment, TSource>, TSource -> Reader<TEnvironment, TSelector>, (TSource, TSelector) -> TResult) -> Reader<TEnvironment, TResult>
        public static Reader<TEnvironment, TResult> SelectMany<TEnvironment, TSource, TSelector, TResult>(
            this Reader<TEnvironment, TSource> source,
            Func<TSource, Reader<TEnvironment, TSelector>> selector,
            Func<TSource, TSelector, TResult> resultSelector) =>
                environment =>
                {
                    TSource value = source(environment);
                    return resultSelector(value, selector(value)(environment));
                };

        // Wrap: TSource -> Reader<TEnvironment, TSource>
        public static Reader<TEnvironment, TSource> ToReader<TEnvironment, TSource>(this TSource value) =>
            environment => value;

        // Select: (Reader<TEnvironment, TSource>, TSource -> TResult) -> Reader<TEnvironment, TResult>
        public static Reader<TEnvironment, TResult> Select<TEnvironment, TSource, TResult>(
            this Reader<TEnvironment, TSource> source, Func<TSource, TResult> selector) =>
                source.SelectMany(value => selector(value).ToReader<TEnvironment, TResult>(), (value, result) => result);
    }
}
