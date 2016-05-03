using System;

namespace MiniPascal.Parser.AST
{
    public sealed class IntegerOverflowException : Exception
    {
        public IntegerOverflowException()
        {
        }
    }
}
