using NUnit.Framework;
using System;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public sealed class HelloWorldTest : RedirectedOutputTest
    {
        private void ProcessProgram<T>(T InputOutput)
        {
            string inout = InputOutput.ToString();
            ProcessProgram('"' + inout + '"', inout);
        }

        private void ProcessProgram<T>(string Input, T ExpectedOutput)
        {
            string totalInput = "begin writeln(" + Input + ");end.";
            Redirect(totalInput);
            Assert.AreEqual(ExpectedOutput.ToString(), output, totalInput);
        }

        [Test]
        public void PrintHello()
        {
            const string input = @"""Hello world""";
            string expected = "Hello world";
            ProcessProgram(input, expected);
        }

        [Test]
        public void PrintHelloNewLine()
        {
            const string input = @"""Hello world!\nHello you too!\n""";
            string expected = "Hello world!" + Environment.NewLine + "Hello you too!" + Environment.NewLine;
            //Windows newline: /r/n
            ProcessProgram(input, expected);
        }

        [Test]
        public void PrintInt()
        {
            ProcessProgram(23487234);
            ProcessProgram(0);
            ProcessProgram(int.MinValue);
        }

        [Test]
        public void PrintBool()
        {
            ProcessProgram(false);
            ProcessProgram(true);
        }

        [Test]
        public void PrintReal()
        {
            ProcessProgram(123.3555f);
            ProcessProgram(float.MinValue);
        }
    }
}
