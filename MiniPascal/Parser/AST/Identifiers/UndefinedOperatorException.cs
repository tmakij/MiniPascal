using System;

namespace MiniPL.Parser.AST
{
    public sealed class UndefinedOperatorException : Exception
    {
        public MiniPLType Type { get; }
        public OperatorType Operator { get; }

        public UndefinedOperatorException(MiniPLType Type, OperatorType Operator)
        {
            this.Type = Type;
            this.Operator = Operator;
        }
    }
}
