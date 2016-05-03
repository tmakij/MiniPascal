using System;

namespace MiniPascal.Lexer
{
    public sealed class LexerException : Exception
    {
        public LexerException(string Message)
            : base(Message)
        {
        }
    }
}
