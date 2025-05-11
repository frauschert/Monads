using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monads;

namespace MonadsTest
{
    internal class WriterTests
    {
        [Test]
        public void Test1()
        {
            var computation = from x in Writer.Return(100)
                              from y in Writer.Return(200)
                              from _ in Writer.Put($"The sum is: {x + y}")
                              select x + y;
            var (result, output) = computation();
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(300));
                Assert.That(output, Is.EqualTo(new[] { "The sum is: 300" }));
            });
        }
    }
}
