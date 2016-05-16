using NUnit.Framework;
using MiniPascal.Parser.AST;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public sealed class ProcedureTest : RedirectedOutputTest
    {
        [Test]
        public void CallBoolean()
        {
            Redirect("begin procedure hello(i:boolean);begin writeln(\"HelloWorld\",i);end; hello(false);hello(true) end.");
            Assert.AreEqual("HelloWorldFalseHelloWorldTrue", output);
        }

        [Test]
        public void CallBooleanFalse()
        {
            Assert.Catch<TypeMismatchException>(() => Redirect("begin procedure hello(i:boolean);begin writeln(\"HelloWorld\",i);end; hello(1); end."));
        }

        [Test]
        public void CallString()
        {
            Redirect("begin procedure hello(i:string);begin writeln(i);end; hello(\"Hello World!\");hello(\"Hello you too!\") end.");
            Assert.AreEqual("Hello World!Hello you too!", output);
        }

        [Test]
        public void CallManyParameters()
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
        public void CallAndAssign()
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
            Assert.AreEqual("10 4", output);
        }

        [Test]
        public void CallAndAssignEveryType()
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
            Assert.AreEqual("1003,14Truetest__Addition__", output);
        }

        [Test]
        public void CallAndAssignPreviousScope()
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
            Assert.AreEqual("150450", output);
        }
    }
}
