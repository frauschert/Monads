using Monads;
using Monads.Extensions;
using Monads.Linq;
using NUnit.Framework;
using System;

namespace MonadsTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        State<int, int> state = 5.State(); // Create a state with initial value 5.
        State<int, int> result = state.SelectMany(x => State<int, int>.State(x + 1), (x, y) => x + y);
        (int value, int newState) = result(0);
        Assert.AreEqual(11, value); // 5 + 1 + 5 = 11
    }
}
