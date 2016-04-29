using System;

namespace MiniPL.Parser.AST
{
    public sealed class TypeMismatchException : Exception
    {
        public MiniPLType Expected { get; }
        public MiniPLType Found { get; }

        public TypeMismatchException(MiniPLType Expected, MiniPLType Found)
        {
            this.Expected = Expected;
            this.Found = Found;
        }
    }
}
