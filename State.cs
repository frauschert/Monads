using System;

namespace Monads
{
    public delegate (T Value, TState State) State<TState, T>(TState state);
}
