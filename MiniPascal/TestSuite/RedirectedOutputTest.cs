using System;
using System.IO;
using NUnit.Framework;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public abstract class RedirectedOutputTest
    {
        protected string output { get { return writer.ToString(); } }
        private StringWriter writer;

        [SetUp]
        public void Redirect()
        {
            writer = new StringWriter();
            Console.SetOut(writer);
        }
    }
}
