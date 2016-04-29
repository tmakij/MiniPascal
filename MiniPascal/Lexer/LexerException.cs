using System;

namespace MiniPL.Lexer
{
    public sealed class LexerException : Exception
    {
        public LexerException(string Message)
            : base(Message)
        {
        }
    }
}
