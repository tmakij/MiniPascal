using System;

namespace MiniPascal.Parser.AST
{
    public sealed class ArrayAssigmentExpection : Exception
    {
        public SimpleType Expected { get; }
        public MiniPascalType Found { get; }

        public ArrayAssigmentExpection(SimpleType Expected, MiniPascalType Found)
        {
            this.Expected = Expected;
            this.Found = Found;
        }
    }
}
