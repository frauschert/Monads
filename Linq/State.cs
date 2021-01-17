using System;
using System.Collections.Generic;
using System.Text;

namespace Monads.Linq
{
    public static partial class StateExt
    {
        public static State<TState, TResult> SelectMany<TState, TSource, TSelector, TResult>(
            this State<TState, TSource> source,
            Func<TSource, State<TState, TSelector>> selector,
            Func<TSource, TSelector, TResult> resultSelector) =>
            oldState =>
            {
                (TSource Value, TState State) value = source(oldState);
                (TSelector Value, TState State) result = selector(value.Value)(value.State);
                TState newState = result.State;
                return (resultSelector(value.Value, result.Value), newState); // Output new state.
            };

        public static State<TState, TResult> SelectMany<TState, TSource, TResult>(
            this State<TState, TSource> source,
            Func<TSource, State<TState, TResult>> selector) =>
            oldState =>
            {
                (TSource Value, TState State) value = source(oldState);
                return selector(value.Value)(value.State);
            };

        public static State<TState, TResult> Select<TState, TSource, TResult>(
            this State<TState, TSource> source,
            Func<TSource, TResult> selector) => SelectMany<TState, TSource, TResult>(source, value => state => (selector(value), state));

        public static State<TState, TSource> State<TState, TSource>(this TSource value) =>
            oldState => (value, oldState); // Output old state.

        // GetState: () -> State<TState, TState>
        public static State<TState, TState> GetState<TState>() =>
            oldState => (oldState, oldState); // Output old state.

        // SetState: TState -> State<TState, Unit>
        public static State<TState, Unit> SetState<TState>(TState newState) =>
            oldState => (default, newState); // Output new state.
    }
}
