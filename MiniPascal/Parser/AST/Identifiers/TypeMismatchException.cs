using System;

namespace MiniPascal.Parser.AST
{
    public sealed class TypeMismatchException : Exception
    {
        public MiniPascalType Expected { get; }
        public MiniPascalType Found { get; }

        public TypeMismatchException(MiniPascalType Expected, MiniPascalType Found)
        {
            this.Expected = Expected;
            this.Found = Found;
        }
    }
}
