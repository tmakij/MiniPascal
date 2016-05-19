using NUnit.Framework;

namespace MiniPascal.TestSuite
{
    [TestFixture]
    public sealed class WhileTest : RedirectedOutputTest
    {
        [Test]
        public void WhileSimple()
        {
            Redirect(@"begin
                         var i:integer;
                         i:=0;
                         while i<10 do
                            i:=i+1;
                         writeln(i);
                         end.");
            Assert.AreEqual(10.ToString(), output);
        }

        [Test]
        public void WhileSimpleScoped()
        {
            Redirect(@"begin
                         var i:integer;
                         i:=-4;
                         while i<10 do
                            begin
                            writeln(i);
                            i:=i+1;
                            end;
                         end.");
            Assert.AreEqual(string.Concat(new[] { -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }), output);
        }

        [Test]
        public void WhileLessSimple()
        {
            Redirect(@"	begin
		                var i,d:integer;
		                i:=0;
		                d:=250;
		                function asd(b : integer) : integer;
			                begin 
				                i:= i+ 1;
				                return d;
			                end;
		                while i < asd(d) do
			                d := d - 1;
		                writeln(i);
                    end
                .");
            Assert.AreEqual("126", output);
        }
    }
}
