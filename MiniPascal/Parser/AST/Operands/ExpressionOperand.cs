namespace MiniPL.Parser.AST
{
    public sealed class ExpressionOperand : IOperand
    {
        private readonly IExpression expression;

        public ExpressionOperand(IExpression Expression)
        {
            expression = Expression;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            expression.CheckIdentifiers(Used);
        }

        public MiniPascalType NodeType(IdentifierTypes Types)
        {
            return expression.NodeType(Types);
        }

        public void EmitIR(CILEmitter Emitter)
        {
            expression.EmitIR(Emitter);
        }

        public ReturnValue Execute(Variables Global)
        {
            return expression.Execute(Global);
        }
    }
}
