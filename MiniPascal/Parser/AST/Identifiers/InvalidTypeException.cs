using System;

namespace MiniPL.Parser.AST
{
    public sealed class InvalidTypeException : Exception
    {
        public MiniPascalType[] Expected { get; }
        public MiniPascalType Found { get; }

        public InvalidTypeException(MiniPascalType Found, params MiniPascalType[] Expected)
        {
            this.Expected = Expected;
            this.Found = Found;
        }
    }
}
