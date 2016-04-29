using System;

namespace MiniPL.Parser.AST
{
    public sealed class UninitializedVariableException : Exception
    {
        public Identifier Identifier { get; }

        public UninitializedVariableException(Identifier Identifier)
        {
            this.Identifier = Identifier;
        }
    }
}
