using MiniPascal.Parser.AST;
using MiniPascal.Lexer;

namespace MiniPascal
{
    public sealed class Compiler
    {
        public static AbstractSyntaxTree Compile(SourceStream Source, string SaveLocation)
        {
            Scanner scanner = new Scanner(Source);
            TokenStream tokens = scanner.GenerateTokens();
            var parser = new Parser.Parser(tokens);
            AbstractSyntaxTree tree = parser.Parse();
            tree.CheckIdentifiers();
            tree.CheckTypes();
            tree.GenerateCIL(SaveLocation);
            return tree;
        }
    }
}
