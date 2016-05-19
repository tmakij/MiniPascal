using NUnit.Framework;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public sealed class IfTest : RedirectedOutputTest
    {
        [Test]
        public void IfPrint()
        {
            Redirect(@"begin var b : boolean;
                       b := true;
                       if b then writeln(""PRINTED CORRECTLY"") end.");
            Assert.AreEqual("PRINTED CORRECTLY", output);
        }

        [Test]
        public void IfComplexConditionPrint()
        {
            Redirect(@"begin var b : boolean;
                       b := true;
                       var s : string;
                       s := ""a"";
                       var i : integer;
                       i := 56;
                       if (s < ""b"") and (i * 2 > 111) then writeln(""PRINTED CORRECTLY"") end.");
            Assert.AreEqual("PRINTED CORRECTLY", output);
        }

        [Test]
        public void IfPrintElse()
        {
            Redirect(@"begin var b : boolean;
                       b := false;
                       if b then writeln(""PRINTED INCORRECTLY"")
                       else writeln(""CORRECT ANSWER"") end.");
            Assert.AreEqual("CORRECT ANSWER", output);
        }
    }
}
