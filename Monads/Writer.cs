using System;
using System.Collections.Generic;
using System.Linq;

namespace Monads
{
    public delegate (T Value, IEnumerable<TContent> Output) Writer<T, TContent>();

    public static class Writer
    {
        public static Writer<T, TContent> Return<T, TContent>(
            T value, 
            IEnumerable<TContent> output) => 
                () => (value, output);

        public static Writer<T, TContent> Return<T, TContent>(
            T value) =>
                () => (value, Enumerable.Empty<TContent>());

        public static Writer<T, string> Return<T>(
            T value) =>
                () => (value, Enumerable.Empty<string>());

        public static Writer<T, string> Return<T>(
            T value,
            string output) =>
                () => (value, new[] { output });

        public static Writer<T, string> Return<T>(
            T value,
            IEnumerable<string> output) =>
                () => (value, output);

        public static Writer<Unit, string> Put(string value) =>
            () => (default, new[] { value });

        public static Writer<(T Value1, T2 Value2), TContent> Combine<T, T2, TContent>(
            Writer<T, TContent> writer1,
            Writer<T2, TContent> writer2) =>
                () =>
                {
                    var (value1, output1) = writer1();
                    var (value2, output2) = writer2();
                    return ((value1, value2), output1.Concat(output2));
                }; 
    }

    public static class WriterExtensions
    {
        public static Writer<TResult, TContent> Select<TSource, TContent, TResult>(
            this Writer<TSource, TContent> writer,
            Func<TSource, TResult> selector) =>
                () =>
                {
                    var (value, output) = writer();
                    return (selector(value), output);
                };

        public static Writer<TResult, TContent> SelectMany<TSource, TContent, TResult>(
            this Writer<TSource, TContent> writer, 
            Func<TSource, Writer<TResult, TContent>> selector) =>
                () =>
                {
                    var (value, output) = writer();
                    var (nextValue, nextOutput) = selector(value)();
                    return (nextValue, output.Concat(nextOutput));
                };

        public static Writer<TResult, TContent> SelectMany<TSource, TContent, TSelector, TResult>(
            this Writer<TSource, TContent> source,
            Func<TSource, Writer<TSelector, TContent>> selector,
            Func<TSource, TSelector, TResult> resultSelector) =>
                () =>
                {
                    var (value, output) = source();
                    var (nextValue, nextOutput) = selector(value)();
                    return (resultSelector(value, nextValue), output.Concat(nextOutput));
                };
    }
}
