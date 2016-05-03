using System;

namespace MiniPascal.Parser.AST
{
    public sealed class UndefinedOperatorException : Exception
    {
        public MiniPascalType Type { get; }
        public OperatorType Operator { get; }

        public UndefinedOperatorException(MiniPascalType Type, OperatorType Operator)
        {
            this.Type = Type;
            this.Operator = Operator;
        }
    }
}
