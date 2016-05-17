using NUnit.Framework;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public sealed class ArrayTest : RedirectedOutputTest
    {
        [Test]
        public void TestZero()
        {
            Redirect(@"begin
                        var arr : array[1] of integer;
                        arr[0] := 5111;
                        writeln(arr[0]) end.");
            Assert.AreEqual(5111.ToString(), output);
        }

        [Test]
        public void TestMultipleAssigments()
        {
            Redirect(@"begin
                        var arr : array[100] of integer;
                        arr[0]:=1;
                        arr[50] := 1;
                        arr[76] := 2;
                        arr[99] := 5;
                        arr[arr[50]+arr[76]] := arr[99] * 1000 + arr[0];
                        writeln(arr[3]) end.");
            Assert.AreEqual(5001.ToString(), output);
        }

        [Test]
        public void TestArrayReassigment()
        {
            Redirect(@"begin
                        var arr : array[100] of integer;
                        arr[0]:=1;
                        procedure aas(i:integer); begin arr[0] := 2 end;
                        aas(0);
                        writeln(arr[0]) end.");
            Assert.AreEqual("2", output);
        }

        [Test]
        public void TestArrayVariableReassigment()
        {
            Redirect(@"begin
                        var arr : array[100] of integer;
                        arr[0]:=1;
                        procedure aas(i:integer); begin var arr2 : array[1] of integer; arr2[0] :=3;arr := arr2; end;
                        aas(0);
                        writeln(arr[0]) end.");
            Assert.AreEqual("3", output);
        }
    }
}
