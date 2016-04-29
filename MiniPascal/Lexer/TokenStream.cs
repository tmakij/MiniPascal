using System.Collections.Generic;

namespace MiniPL.Lexer
{
    public sealed class TokenStream
    {
        public Token Current { get; private set; }
        public Symbol Symbol { get; private set; }
        private readonly IList<Token> tokens;
        private int position;

        public TokenStream(IList<Token> Tokens)
        {
            tokens = Tokens;
            UpdateCurrent();
        }

        public void Next()
        {
            position++;
            UpdateCurrent();
        }

        private void UpdateCurrent()
        {
            Current = tokens[position];
            Symbol = Current.Symbol;
        }
    }
}
