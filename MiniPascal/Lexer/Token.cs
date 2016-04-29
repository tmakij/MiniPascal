namespace MiniPL.Lexer
{
    public sealed class Token
    {
        public Symbol Symbol { get; }
        public string Lexeme { get; }

        public Token(string Lexeme, Symbol Symbol)
        {
            this.Lexeme = Lexeme;
            this.Symbol = Symbol;
        }
    }
}
