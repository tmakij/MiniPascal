using NUnit.Framework;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public sealed class ArrayTest : RedirectedOutputTest
    {
        [Test]
        public void ArraysZero()
        {
            Redirect(@"begin
                        var arr : array[1] of integer;
                        arr[0] := 5111;
                        writeln(arr[0]) end.");
            Assert.AreEqual(5111.ToString(), output);
        }

        [Test]
        public void ArraysMultipleAssigments()
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
        public void ArraysReassigment()
        {
            Redirect(@"begin
                        var arr : array[100] of integer;
                        arr[0]:=1;
                        procedure aas(i:integer); begin arr[0] := 2 end;
                        aas(0);
                        writeln(arr[0]) end.");
            Assert.AreEqual(2.ToString(), output);
        }

        [Test]
        public void ArraysVariableReassigment()
        {
            Redirect(@"begin
                        var arr : array[100] of integer;
                        arr[0]:=1;
                        procedure aas(i:integer); begin var arr2 : array[1] of integer; arr2[0] :=3;arr := arr2; end;
                        aas(0);
                        writeln(arr[0]) end.");
            Assert.AreEqual(3.ToString(), output);
        }

        [Test]
        public void ArraysIndexByRef()
        {
            Redirect(@"begin
                        var arr : array[100] of integer;
                        var st : array[2] of string;
                        arr[0]:=1;
                        procedure aas(var i:integer);
                            begin
                                i := 2;
                                st[1] := \""Hello\"";
                            end;
                        aas(arr[0]);
                        writeln(arr[0], st[1]) end.");
            Assert.AreEqual(2 + "Hello", output);
        }

        [Test]
        public void ArraysSize()
        {
            Redirect(@"begin
                        var arr : array[100] of integer;
                        var arr2 : array[12322] of integer;
                        writeln(arr.size, arr2.size) end.");
            Assert.AreEqual(100 + "" + 12322, output);
        }

        [Test]
        public void ArraysReal()
        {
            Redirect(@"begin
                        var arr : array[100] of real;
                        arr[5]:= 3.14;
                        writeln(arr[5], \"" \"", arr[0]) end.");
            Assert.AreEqual(3.14 + " " + 0f, output);
        }

        [Test]
        public void ArraysBoolean()
        {
            Redirect(@"begin
                        var arr : array[3] of boolean;
                        arr[0]:= true;
                        arr[1]:= true;
                        arr[2]:= false;
                        writeln(arr[0], arr[1], arr[2]) end.");
            Assert.AreEqual(bool.TrueString + bool.TrueString + bool.FalseString, output);
        }

        [Test]
        public void ArraysString()
        {
            Redirect(@"begin
                        var arr : array[3] of string;
                        arr[0]:= \""hello \"";
                        arr[1]:= \""world\"";
                        arr[2]:= \""!\"";
                        writeln(arr[0], arr[1], arr[2]) end.");
            Assert.AreEqual("hello world!", output);
        }

        [Test]
        public void ArraysEmptyString()
        {
            Redirect(@"begin
                        var arr : array[3] of string;
                        writeln(\""\"" = arr[0],\""\"" <> arr[1], arr[2]) end.");
            Assert.AreEqual(bool.FalseString + bool.TrueString, output);
        }

        [Test]
        public void ArraysInvalidIndex()
        {
            Assert.Catch<Parser.AST.InvalidArrayIndexTypeException>(() =>
            {
                Redirect(@"begin
                        var arr, arr2 : array[2] of integer;
                        arr[arr2] := 2; end.");
            });
        }

        [Test]
        public void ArraysInvalidIndexNoType()
        {
            Assert.Catch<Parser.AST.InvalidArrayIndexTypeException>(() =>
            {
                Redirect(@"begin
                        var arr, arr2 : array[2] of integer;
                        arr[] := 2; end.");
            });
        }

        [Test]
        public void ArraysInvalidSize()
        {
            Assert.Catch<Parser.AST.ArrayRequiredException>(() =>
            {
                Redirect(@"begin writeln((1).size); end.");
            });
        }
    }
}
