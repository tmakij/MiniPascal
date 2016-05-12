using NUnit.Framework;

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
            Assert.Catch<Parser.AST.TypeMismatchException>(() => Redirect("begin procedure hello(i:boolean);begin writeln(\"HelloWorld\",i);end; hello(1); end."));
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
                    procedure hello(i:string,j:integer,k:integer,l:boolean,o:real);
                        begin
                            writeln(i,k,l,o,j *2);
                        end;
                    hello(""Hello World!"", 50, 13,false,12.556);
                end.");
            Assert.AreEqual("Hello World!" + 13 + "" + false + "" + 12.556 + "" + (50 * 2), output);
        }
    }
}
