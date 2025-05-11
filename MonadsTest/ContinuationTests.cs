using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monads;

namespace MonadsTest
{
    internal class ContinuationTests
    {
        [Test]
        public void Test1()
        {
            var cps = 5.Cps<string, int>();
            var result = cps(x => x.ToString());
            Assert.That(result, Is.EqualTo("5")); // Check the result of the continuation.
        }
    }
}
