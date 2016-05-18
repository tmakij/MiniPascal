using NUnit.Framework;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public sealed class FunctionTest : RedirectedOutputTest
    {
        [Test]
        public void TestSimpleCall()
        {
            Redirect(@"begin
                        function pr(i:boolean) : real;
                            begin
                                  if(i) then return 1.25
                                  else return -5.0
                            end;
                         writeln(pr(true));
                         writeln(pr(false));
                         end.");
            Assert.AreEqual(1.25f + "" + -5.0f, output);
        }

        [Test]
        public void TestComplexCall()
        {
            Redirect(@"begin
                        var t : integer;
                        t := 5;
                        function pr(i:integer, var r : real) : real;
                            begin
                                  t := 15;
                                  r := -2.0;
                                  if(i >3) then return 5.0e4
                                  else return -5.0 + r;
                                  t := 534;
                            end;
                         var a,b : real;
                         b := 2.5e3;
                         a := pr(4, b) + 5.0;
                         writeln(t, b, a, pr(1,a) / 10.0);
                         end.");
            Assert.AreEqual(15 + "" + -2f + "" + 50005f + "" + -0.7f, output);
        }
    }
}
