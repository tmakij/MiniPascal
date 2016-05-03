using System;
using MiniPascal.Lexer;

namespace MiniPascal.Parser
{
    public sealed class SyntaxException : Exception
    {
        public SyntaxException(string Message)
            : base(Message)
        {
        }

        public SyntaxException(string Expected, Symbol Found)
            : this("Expected " + Expected + ", but " + Found + " was found")
        {
        }
    }
}
