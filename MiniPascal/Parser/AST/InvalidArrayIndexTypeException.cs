using System;

namespace MiniPascal.Parser.AST
{
    public sealed class InvalidArrayIndexTypeException : Exception
    {
        public MiniPascalType Found { get; }

        public InvalidArrayIndexTypeException(MiniPascalType Found)
        {
            this.Found = Found;
        }
    }
}
