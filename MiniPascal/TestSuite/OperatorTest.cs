using NUnit.Framework;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public sealed class OperatorTest : RedirectedOutputTest
    {
        [Test]
        public void OperatorsInteger()
        {
            Redirect(@"begin writeln(1+4, \"" \"", 2+7*5, \"" \"", (10-2)/4, \"" \"", 51/10, \"" \"", 25 * 5 % 5, \"" \"", 99 % 5) end.");
            Assert.AreEqual(5 + " " + 37 + " " + 2 + " " + 5 + " " + 0 + " " + 4, output);
        }

        [Test]
        public void OperatorsReal()
        {
            Redirect(@"begin writeln(1.1+4.1, \"" \"", 10.0*5.55 / 2.0, \"" \"", 10.2-2.1, \"" \"", 1.0+ 1.5/0.5, \"" \"", 0.1 * 150.0 - 1.0) end.");
            Assert.AreEqual(5.2f + " " + 27.75f + " " + 8.1f + " " + 4.0f + " " + 14.0f, output);
        }

        [Test]
        public void OperatorsStringConcat()
        {
            Redirect(@"begin writeln(\""hello\"" + \"" world!\"" + \""\"" + \""dsaasdaaaaaaaaaaaaaaaaaaaaaaaaa\"") end.");
            Assert.AreEqual("hello world!dsaasdaaaaaaaaaaaaaaaaaaaaaaaaa", output);
        }

        [Test]
        public void OperatorsStringRelation()
        {
            Redirect(@"begin writeln(\""a\"" < \""b\"", \""a\"" <= \""a\"", \""a\"" = \""a\"", \""a\"" <> \""a\"", \""b\"" > \""a\"", \""a\"" >= \""a\"") end.");
            Assert.AreEqual(bool.TrueString + bool.TrueString + bool.TrueString + bool.FalseString + bool.TrueString + bool.TrueString, output);
        }

        [Test]
        public void OperatorsStringRelationReverse()
        {
            Redirect(@"begin writeln(\""a\"" > \""b\"", \""b\"" <= \""a\"", \""b\"" = \""a\"", \""b\"" <> \""a\"", \""a\"" > \""b\"", \""a\"" >= \""b\"") end.");
            Assert.AreEqual(bool.FalseString + bool.FalseString + bool.FalseString + bool.TrueString + bool.FalseString + bool.FalseString, output);
        }

        [Test]
        public void OperatorsBoolean()
        {
            Redirect(@"begin writeln(false and false, true and true, true and false, false and true,
                                    false or false, true or true, true or false, false or true,
                                    not true, not false) end.");
            Assert.AreEqual(bool.FalseString + bool.TrueString + bool.FalseString + bool.FalseString +
                            bool.FalseString + bool.TrueString + bool.TrueString + bool.TrueString +
                            bool.FalseString + bool.TrueString, output);
        }
    }
}
