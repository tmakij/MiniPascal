using MiniPascal.Parser.AST;
using MiniPascal.Lexer;

namespace MiniPascal
{
    public sealed class Compiler
    {
        public static void Compile(SourceStream Source)
        {
            Scanner scanner = new Scanner(Source);
            TokenStream tokens = scanner.GenerateTokens();
            var parser = new MiniPascal.Parser.Parser(tokens);
            AbstractSyntaxTree tree = parser.Parse();
            tree.CheckIdentifiers();
            tree.CheckTypes();
            //tree.Execute();
            tree.GenerateCIL();
        }
    }
}
