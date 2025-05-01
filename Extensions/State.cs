using System;
using System.Collections.Generic;
using System.Text;

namespace Monads.Extensions
{
    public static partial class StateExt
    {
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
