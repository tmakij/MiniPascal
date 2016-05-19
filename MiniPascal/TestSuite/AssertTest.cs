using System;
using NUnit.Framework;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public sealed class AssertTest : RedirectedOutputTest
    {
        [Test]
        public void AssertSucceed()
        {
            Redirect(@"begin
                         assert(true);
                         writeln(""HelloWorld!"");
                         end.");
            Assert.AreEqual("HelloWorld!", output);
        }

        [Test]
        public void AssertFail()
        {
            Redirect(@"begin
                         assert(false);
                         writeln(""HelloWorld!"");
                         end.");
            Assert.AreEqual(Environment.NewLine + "Assert failure" + Environment.NewLine, output);
        }

        [Test]
        public void AssertPartialFail()
        {
            Redirect(@"begin
                         writeln(""Hello"");
                         assert(true);
                         writeln(""World"");
                         assert(false);
                         writeln(""!"");
                         end.");
            Assert.AreEqual("HelloWorld" + Environment.NewLine + "Assert failure" + Environment.NewLine, output);
        }
    }
}
