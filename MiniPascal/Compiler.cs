using MiniPL.Parser.AST;
using MiniPL.Lexer;

namespace MiniPL
{
    public sealed class Compiler
    {
        public static void Compile(SourceStream Source)
        {
            Scanner scanner = new Scanner(Source);
            TokenStream tokens = scanner.GenerateTokens();
            var parser = new MiniPL.Parser.Parser(tokens);
            AbstractSyntaxTree tree = parser.Parse();
            tree.CheckIdentifiers();
            tree.CheckTypes();
            //tree.Execute();
            tree.GenerateCIL();
        }
    }
}
