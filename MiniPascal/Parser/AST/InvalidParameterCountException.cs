using System;

namespace MiniPascal.Parser.AST
{
    public sealed class InvalidParameterCountException : Exception
    {
        public int Expected { get; }
        public int Found { get; }

        public InvalidParameterCountException(int Expected, int Found)
        {
            this.Expected = Expected;
            this.Found = Found;
        }
    }
}
