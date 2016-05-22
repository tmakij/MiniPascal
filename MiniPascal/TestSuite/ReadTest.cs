using NUnit.Framework;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public sealed class ReadTest : RedirectedOutputTest
    {
        [Test]
        public void ReadString()
        {
            Redirect(@"begin
                         var i :string;
                         read(i);
                         writeln(i);
                         end.", "HelloWorld");
            Assert.AreEqual("HelloWorld", output);
        }

        [Test]
        public void ReadInt()
        {
            Redirect(@"begin
                         var i :integer;
                         read(i);
                         writeln(i);
                         end.", "123");
            Assert.AreEqual(123.ToString(), output);
        }

        [Test]
        public void ReadBoolean()
        {
            Redirect(@"begin
                         var i :boolean;
                         read(i);
                         writeln(i);
                         end.", "fAlSE");
            Assert.AreEqual(bool.FalseString, output);
        }

        [Test]
        public void ReadReals()
        {
            Redirect(@"begin
                         var i,d :real;
                         read(i,d);
                         writeln(i, d);
                         end.", "-434.5667e4", "1.23");
            Assert.AreEqual(-434.5667e4f + "" + 1.23f, output);
        }
    }
}
