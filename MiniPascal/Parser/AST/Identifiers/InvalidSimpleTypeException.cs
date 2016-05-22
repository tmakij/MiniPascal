using System;

namespace MiniPascal.Parser.AST
{
    public sealed class InvalidSimpleTypeException : Exception
    {
        public SimpleType Expected { get; }
        public SimpleType Found { get; }

        public InvalidSimpleTypeException(SimpleType Found, SimpleType Expected)
        {
            this.Expected = Expected;
            this.Found = Found;
        }
    }
}
