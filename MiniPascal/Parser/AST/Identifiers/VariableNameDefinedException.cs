using System;

namespace MiniPascal.Parser.AST
{
    public sealed class VariableNameDefinedException : Exception
    {
        public Identifier Identifier { get; }

        public VariableNameDefinedException(Identifier Identifier)
        {
            this.Identifier = Identifier;
        }
    }
}
