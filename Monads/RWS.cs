using System;
using System.Collections.Generic;
using System.Linq;

namespace Monads
{
    public delegate (T Value, TState State, IEnumerable<TLog> Log) RWS<TEnvironment, TState, TLog, T>(TEnvironment environment, TState state);

    public static class RWS
    {
        // Create: T -> RWS<TEnvironment, TState, TLog, T>
        public static RWS<TEnvironment, TState, TLog, T> Return<TEnvironment, TState, TLog, T>(T value) =>
            (env, state) => (value, state, default);

        // Read: (TEnvironment -> T) -> RWS<TEnvironment, TState, TLog, T>
        public static RWS<TEnvironment, TState, TLog, T> Read<TEnvironment, TState, TLog, T>(Func<TEnvironment, T> reader) =>
            (env, state) => (reader(env), state, default);

        // Write: TLog -> RWS<TEnvironment, TState, TLog, Unit>
        public static RWS<TEnvironment, TState, TLog, Unit> Write<TEnvironment, TState, TLog>(TLog log) =>
            (env, state) => (default, state, new[] { log });

        // GetState: () -> RWS<TEnvironment, TState, TLog, TState>
        public static RWS<TEnvironment, TState, TLog, TState> GetState<TEnvironment, TState, TLog>() =>
            (env, state) => (state, state, default);

        // SetState: TState -> RWS<TEnvironment, TState, TLog, Unit>
        public static RWS<TEnvironment, TState, TLog, Unit> SetState<TEnvironment, TState, TLog>(TState newState) =>
            (env, state) => (default, newState, default);

        // SelectMany: (RWS<TEnvironment, TState, TLog, TSource>, TSource -> RWS<TEnvironment, TState, TLog, TSelector>, (TSource, TSelector) -> TResult) -> RWS<TEnvironment, TState, TLog, TResult>
        public static RWS<TEnvironment, TState, TLog, TResult> SelectMany<TEnvironment, TState, TLog, TSource, TSelector, TResult>(
            this RWS<TEnvironment, TState, TLog, TSource> source,
            Func<TSource, RWS<TEnvironment, TState, TLog, TSelector>> selector,
            Func<TSource, TSelector, TResult> resultSelector) =>
            (env, state) =>
            {
                var (value1, state1, log1) = source(env, state);
                var (value2, state2, log2) = selector(value1)(env, state1);
                return (resultSelector(value1, value2), state2, log1.Concat(log2));
            };

        // Select: (RWS<TEnvironment, TState, TLog, TSource>, TSource -> TResult) -> RWS<TEnvironment, TState, TLog, TResult>
        public static RWS<TEnvironment, TState, TLog, TResult> Select<TEnvironment, TState, TLog, TSource, TResult>(
            this RWS<TEnvironment, TState, TLog, TSource> source,
            Func<TSource, TResult> selector) =>
            source.SelectMany(value => Return<TEnvironment, TState, TLog, TResult>(selector(value)), (value, result) => result);
    }
}
