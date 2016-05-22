using System;

namespace MiniPascal.Parser.AST
{
    public sealed class ArrayRequiredException : Exception
    {
        public MiniPascalType Found { get; }

        public ArrayRequiredException(MiniPascalType Found)
        {
            this.Found = Found;
        }
    }
}
