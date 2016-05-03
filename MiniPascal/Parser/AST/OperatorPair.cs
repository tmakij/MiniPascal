namespace MiniPascal.Parser.AST
{
    public sealed class OperatorPair<T>
    {
        public OperatorType Operator { get; }
        public T Operand { get; }

        public OperatorPair(OperatorType Operator, T Operand)
        {
            this.Operator = Operator;
            this.Operand = Operand;
        }
    }
}
