using Monads;

namespace MonadsTest
{
    internal class TryTests
    {
        [Test]
        public void Test1()
        {
            Try<int> try1 = 5;
            var result = try1.SelectMany(x => new Try<int>(() => x + 1), (x, y) => x + y)
                .Match(right => right, left => 1);
            Assert.That(result, Is.EqualTo(11)); // 5 + 1 + 5 = 11
        }

        [Test]
        public void Test2() 
        {
            IO<int> io = () => throw new Exception("Test exception");
            Try<int> try2 = io;
            var result = try2.Catch<int, Exception>(ex => 1, ex => ex.Message == "Test exception").Match(
                right => right,
                left => 0
            );
            Assert.That(result, Is.EqualTo(1));
        }
    }
}
