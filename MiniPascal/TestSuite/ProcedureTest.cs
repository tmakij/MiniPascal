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
            Assert.AreEqual("Hello World!" + 13 + "" + false + "" + 12.556 + "" + (50 * 2), output);
        }

        [Test]
        public void CallAndAssign()
        {
            try
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
            }
            catch (UninitializedVariableException e)
            {
                System.Console.WriteLine(e.Identifier);
            }
            Assert.AreEqual("10 4", output);
        }
    }
}
