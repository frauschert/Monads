using System;
using System.Collections.Generic;
using System.Linq;

namespace Monads
{
    public delegate (T Value, TState State) State<TState, T>(TState state);
    
    public static partial class State
    {
        public static State<TState, TSource> Create<TState, TSource>(TSource value) =>
            oldState => (value, oldState); // Output old state.

        // GetState: () -> State<TState, TState>
        public static State<TState, TState> GetState<TState>() =>
            oldState => (oldState, oldState); // Output old state.

        // SetState: TState -> State<TState, Unit>
        public static State<TState, Unit> SetState<TState>(TState newState) =>
            oldState => (default, newState); // Output new state.

        // Pop: () -> (IEnumerable<T> -> (T, IEnumerable<T>))
        // Pop: Unit -> State<IEnumerable<T>, T>
        public static State<IEnumerable<T>, T> Pop<T>() =>
            oldStack =>
            {
                IEnumerable<T> newStack = oldStack.Take(oldStack.Count() - 1);
                return (newStack.First(), newStack); // Output new state.
            };

        // Push: T -> (IEnumerable<T> -> (Unit, IEnumerable<T>))
        // Push: T -> State<IEnumerable<T>, Unit>
        public static State<IEnumerable<T>, Unit> Push<T>(T value) =>
            oldStack =>
            {
                IEnumerable<T> newStack = oldStack.Concat(new T[] { value });
                return (default, newStack); // Output new state.
            };

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
            Func<TSource, TResult> selector) => 
                SelectMany<TState, TSource, TResult>(source, value => state => (selector(value), state));
    }
}
