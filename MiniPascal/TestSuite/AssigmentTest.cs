using NUnit.Framework;
using System;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public sealed class AssigmentTest : RedirectedOutputTest
    {
        [Test]
        public void AssignInteger()
        {
            Redirect("begin var i:integer;i:=23123;writeln(i) end.");
            Assert.AreEqual("23123", output);
        }

        [Test]
        public void AssignBoolean()
        {
            Redirect("begin var i:boolean;i:=false;writeln(i);i:=true;writeln(i) end.");
            Assert.AreEqual("FalseTrue", output);
        }

        [Test]
        public void AssignReal()
        {
            Redirect("begin var i:real;i:=12.0;writeln(i);i:=-0.576e3;writeln(i) end.");
            Assert.AreEqual("12" + (-0.576e3F).ToString(), output);
        }

        [Test]
        public void AssignString()
        {
            Redirect(@"begin var i:string;i:=\""hello\"";writeln(i);i:=\""\nworld!\"";writeln(i) end.");
            Assert.AreEqual("hello" + Environment.NewLine + "world!", output);
        }

        [Test]
        public void AssignStringFromVar()
        {
            Redirect(@"begin var i,j,k:string;i:=\""hello\"";k:=\""world!\""; j:= i + \""\n\"" +k;writeln(j) end.");
            Assert.AreEqual("hello" + Environment.NewLine + "world!", output);
        }
    }
}
