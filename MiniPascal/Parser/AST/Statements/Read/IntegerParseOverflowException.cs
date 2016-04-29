using System;

namespace MiniPL.Parser.AST
{
    public sealed class IntegerParseOverflowException : Exception
    {
        public string Value { get; }

        public IntegerParseOverflowException(string Value)
        {
            this.Value = Value;
        }
    }
}
