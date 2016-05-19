using NUnit.Framework;
using MiniPascal.Parser.AST;
using Assert = NUnit.Framework.Assert;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public sealed class ProcedureTest : RedirectedOutputTest
    {
        [Test]
        public void ProcedureCallBoolean()
        {
            Redirect("begin procedure hello(i:boolean);begin writeln(\"HelloWorld\",i);end; hello(false);hello(true) end.");
            Assert.AreEqual("HelloWorldFalseHelloWorldTrue", output);
        }

        [Test]
        public void ProcedureCallBooleanFalse()
        {
            Assert.Catch<TypeMismatchException>(() => Redirect("begin procedure hello(i:boolean);begin writeln(\"HelloWorld\",i);end; hello(1); end."));
        }

        [Test]
        public void ProcedureCallString()
        {
            Redirect("begin procedure hello(i:string);begin writeln(i);end; hello(\"Hello World!\");hello(\"Hello you too!\") end.");
            Assert.AreEqual("Hello World!Hello you too!", output);
        }

        [Test]
        public void ProcedureCallManyParameters()
        {
            Redirect(
                @"
                begin 
                    procedure prt(i:string,j:integer,k:integer,l:boolean,o:real);
                        begin
                            writeln(i,k,l,o,j *2);
                        end;
                    prt(""Hello World!"", 50, 13,false,12.556);
                end.");
            Assert.AreEqual("Hello World!" + 13 + "" + false + "" + 12.556f + "" + (50 * 2), output);
        }

        [Test]
        public void ProcedureCallAndAssign()
        {
            Redirect(
                @"
                begin 
                    procedure mul(var a:integer);
                        begin
                            a := 2 * a;
                        end;
                    var i,j : integer;
                    i:=2;
                    j:=5;
                    mul(i);
                    mul(j);
                    writeln(j, "" "", i);
                end.");
            Assert.AreEqual(10 + " " + 4, output);
        }

        [Test]
        public void ProcedureCallAndAssignArray()
        {
            Redirect(
                @"
                begin
                    var arr : array[1] of integer;
                    var arr3 : array[3] of integer;
                    var arrByRef : array[9] of boolean;
                    procedure assign( a: array[] of integer,var byref : array[] of boolean);
                        begin
                            var arr2 : array[2] of integer;
                            arr2[1] := 321;
                            arr := arr2;
                            a[2] := 515;
                            var byrefReplacement : array[11] of boolean;
                            byrefReplacement[10] := true;
                            byref := byrefReplacement;
                        end;
                    assign(arr3, arrByRef);
                    writeln(arr[1], "" "",arr3[2], arrByRef[10]);
                end.");
            Assert.AreEqual(321 + " " + 515 + bool.TrueString, output);
        }

        [Test]
        public void ProcedureCallAndAssignEveryType()
        {
            Redirect(
                @"
                begin 
                    procedure proc(var a:integer, var b :real, var c:boolean,var d : string);
                        begin
                            a := 2 * a;
                            b := 3.14;
                            c := true;
                            d := d + ""__Addition__""
                        end;
                    var i : integer; var r:real;var bb:boolean; var s:string;
                    i:= 50;r:=-1.0;bb:=false;s:=""test"";
                    proc(i,r,bb,s);
                    writeln(i,r,bb,s);
                end.");
            Assert.AreEqual(100 + "" + 3.14 + bool.TrueString + "test__Addition__", output);
        }

        [Test]
        public void ProcedureCallAndAssignPreviousScope()
        {
            Redirect(
                @"
                begin
                    var b, d:integer;
                    procedure mul(a:integer);
                        begin
                            b := 2 * a; d:= b* 3;
                        end;
                    mul(75);
                    writeln(b,d);
                end.");
            Assert.AreEqual(150 + "" + 450, output);
        }

        [Test]
        public void ProcedureCallAndAssignPreviousScopeMany()
        {
            Redirect(
                @"
                begin
                    var b, d:integer;
                    b := -5;
                    d:=1;
                    procedure mul(a:integer);
                        begin
                        procedure proc2(k:boolean);
                            begin
                                b := b * a;
                                d:= 4 * d;
                            end;
                        b := 2 * a;
                        proc2(false);
                        d:= d* b* 3;
                        end;
                    mul(7);
                    writeln(b,d);
                end.");
            Assert.AreEqual(98 + "" + 1176, output);
        }

        [Test]
        public void ProcedureCallScopesOverrideCorrect()
        {
            Redirect(
                @"
                begin
                    var b,c:integer;
                    b := 1;
                    c := -1;
                    procedure mul(a:integer);
                        begin
                        procedure mul(c:integer);
                            begin
                                b := 5;
                                c := 0;
                            end;
                        b := 2;
                        c := 1;
                        mul(0);
                        end;
                    mul(0);
                    writeln(b,c);
                end.");
            Assert.AreEqual(5 + "" + 1, output);
        }

        [Test]
        public void ProcedureCallScopesRecursively()
        {
            Redirect(
                @"
                begin
                    var b:integer;
                    b := 1;
                    procedure mul(a:integer);
                        begin
                        b := b * 2;
                        if (a > 0) then mul(a - 1)
                        end;
                    mul(6);
                    writeln(b);
                end.");
            Assert.AreEqual((2 << 6).ToString(), output);
        }

        [Test]
        public void ProcedureCallScopesRecursivelyStringConcat()
        {
            Redirect(
                @"
                begin
                    var b:string;
                    b := """";
                    procedure mul(a:integer);
                        begin
                        b := b + ""ABC"";
                        if (a > 0) then mul(a - 1)
                        end;
                    mul(6);
                    writeln(b);
                end.");
            Assert.AreEqual("ABCABCABCABCABCABCABC", output);
        }
    }
}
