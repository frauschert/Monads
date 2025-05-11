using Monads;

namespace MonadsTest;

public class Tests
{
    [Test]
    public void Test1()
    {
        State<int, int> state = State.Create<int, int>(5); // Create a state with initial value 5.  
        State<int, int> result = state.SelectMany(x => State.Create<int, int>(x + 1), (x, y) => x + y);
        (int value, int newState) = result(0);
        Assert.That(value, Is.EqualTo(11)); // 5 + 1 + 5 = 11  
    }

    [Test]
    public void Workflow()
    {
        string initialState = nameof(initialState);
        string newState = nameof(newState);
        string resetState = nameof(resetState);
        State<string, int> source1 = oldState => (1, oldState);
        State<string, bool> source2 = oldState => (true, newState);
        State<string, char> source3 = State.Create<string, char>('@'); // oldState => 2, oldState).

        State<string, string[]> query =
            from value1 in source1 // source1: State<string, int> = initialState => (1, initialState).
            from state1 in State.GetState<string>() // GetState<int>(): State<string, string> = initialState => (initialState, initialState).
            from value2 in source2 // source2: State<string, bool>3 = initialState => (true, newState).
            from state2 in State.GetState<string>() // GetState<int>(): State<string, string> = newState => (newState, newState).
            from unit in State.SetState(resetState) // SetState(resetState): State<string, Unit> = newState => (default, resetState).
            from state3 in State.GetState<string>() // GetState(): State<string, string> = resetState => (resetState, resetState).
            from value3 in source3 // source3: State<string, char> = resetState => (@, resetState).
            select new string[] { state1, state2, state3 }; // Define query.
        (string[] Value, string State) result = query(initialState); // Execute query with initial state.
        Assert.That(result.Value.Length, Is.EqualTo(3)); // Check the length of the result array.
        Assert.That(result.Value[0], Is.EqualTo(initialState)); // Check the first element of the result array.
        Assert.That(result.Value[1], Is.EqualTo(newState)); // Check the second element of the result array.
        Assert.That(result.Value[2], Is.EqualTo(resetState)); // Check the third element of the result array.
        Assert.That(result.State, Is.EqualTo(resetState)); // Check the final state.
    }

    [Test]
    public void Test2()
    {
        IEnumerable<int> initialStack = Enumerable.Repeat(0, 5);
        State<IEnumerable<int>, IEnumerable<int>> query =
            from value1 in State.Pop<int>() // State<IEnumerable<int>, int>.
            from unit1 in State.Push(1) // State<IEnumerable<int>, Unit>.
            from unit2 in State.Push(2) // State<IEnumerable<int>, Unit>.
            from stack in State.GetState<IEnumerable<int>>() // State<IEnumerable<int>, IEnumerable<int>>.
            select stack; // Define query.
        (IEnumerable<int> Value, IEnumerable<int> State) result = query(initialStack); // Execute query with initial state.
        Assert.That(result.Value.Count(), Is.EqualTo(6)); // Check the length of the result array.
        Assert.That(result.Value.ElementAt(0), Is.EqualTo(0)); // Check the first element of the result array.
        Assert.That(result.Value.ElementAt(1), Is.EqualTo(0)); // Check the second element of the result array.
        Assert.That(result.Value.ElementAt(2), Is.EqualTo(0)); // Check the third element of the result array.
        Assert.That(result.Value.ElementAt(3), Is.EqualTo(0)); // Check the fourth element of the result array.
        Assert.That(result.Value.ElementAt(4), Is.EqualTo(1)); // Check the fifth element of the result array.
        Assert.That(result.Value.ElementAt(5), Is.EqualTo(2)); // Check the sixth element of the result array.
        Assert.That(result.State.Count(), Is.EqualTo(6)); // Check the length of the result array.
        Assert.That(result.State.ElementAt(0), Is.EqualTo(0)); // Check the first element of the result array.
        Assert.That(result.State.ElementAt(1), Is.EqualTo(0)); // Check the second element of the result array.
        Assert.That(result.State.ElementAt(2), Is.EqualTo(0)); // Check the third element of the result array.
        Assert.That(result.State.ElementAt(3), Is.EqualTo(0)); // Check the fourth element of the result array.
        Assert.That(result.State.ElementAt(4), Is.EqualTo(1)); // Check the fifth element of the result array.
        Assert.That(result.State.ElementAt(5), Is.EqualTo(2)); // Check the sixth element of the result array.
    }
}
