namespace MiniPL.Parser.AST
{
    public sealed class UnaryExpression : IExpression
    {
        private readonly OperatorType expressionOperator;
        private readonly IOperand operand;

        public UnaryExpression(OperatorType Operator, IOperand Operand)
        {
            expressionOperator = Operator;
            operand = Operand;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            operand.CheckIdentifiers(Used);
        }

        public MiniPLType NodeType(IdentifierTypes Types)
        {
            MiniPLType type = operand.NodeType(Types);
            if (expressionOperator != OperatorType.None && !type.HasOperatorDefined(expressionOperator))
            {
                throw new UndefinedOperatorException(type, expressionOperator);
            }
            return type;
        }

        public ReturnValue Execute(Variables Global)
        {
            ReturnValue ret = operand.Execute(Global);
            if (expressionOperator != OperatorType.None)
            {
                object val = ret.Type.UnaryOperation(ret.Value, expressionOperator);
                ret = new ReturnValue(ret.Type, val);
            }
            return ret;
        }
    }
}
