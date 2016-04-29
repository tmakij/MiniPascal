using System;

namespace MiniPL.Parser.AST
{
    public sealed class IntegerFormatException : Exception
    {
        public string ParseAttempt { get; }

        public IntegerFormatException(string ParseAttempt)
        {
            this.ParseAttempt = ParseAttempt;
        }
    }
}
