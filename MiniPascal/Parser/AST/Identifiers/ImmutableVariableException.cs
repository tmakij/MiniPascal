using System;

namespace MiniPascal.Parser.AST
{
    public sealed class ImmutableVariableException : Exception
    {
        public Identifier Identifier { get; }

        public ImmutableVariableException(Identifier Identifier)
        {
            this.Identifier = Identifier;
        }
    }
}
