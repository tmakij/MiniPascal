using NUnit.Framework;
using System.IO;
using MiniPL;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public sealed class HelloWorldTest : RedirectedOutputTest
    {
        const string HelloWorldProgram = " writeln (\"Hello World!\n\"); ";
        /*const string HelloWorldProgram =
@"program helloWorld
begin
    writeln(23);
end.";*/

        private void ProcessProgram(string Input, string ExpectedOutput)
        {
            using (StringReader stringReader = new StringReader(Input))
            {
                SourceStream ss = new SourceStream(stringReader);
                Compiler.Compile(ss);
            }
            Assert.AreEqual(ExpectedOutput, output);
        }

        [Test]
        public void PrintHello()
        {
            ProcessProgram(HelloWorldProgram, "Hello World!\n");
        }

        [Test]
        public void PrintHello2()
        {
            ProcessProgram(HelloWorldProgram, "Hello World!\n");
        }
    }
}
