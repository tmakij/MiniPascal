namespace MiniPascal.Lexer
{
    public sealed class Token
    {
        public Symbol Symbol { get; }
        public string Lexeme { get; }
        public int Line { get; }

        public Token(string Lexeme, Symbol Symbol, int Line)
        {
            this.Lexeme = Lexeme;
            this.Symbol = Symbol;
            this.Line = Line;
        }
    }
}
