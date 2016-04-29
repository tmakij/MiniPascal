using System;

namespace MiniPL.Parser.AST
{
    public sealed class InvalidTypeException : Exception
    {
        public MiniPLType[] Expected { get; }
        public MiniPLType Found { get; }

        public InvalidTypeException(MiniPLType Found, params MiniPLType[] Expected)
        {
            this.Expected = Expected;
            this.Found = Found;
        }
    }
}
