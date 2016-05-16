namespace MiniPascal.Parser.AST
{
    public sealed class ExpressionOperand : IOperand
    {
        private readonly IExpression expression;

        public ExpressionOperand(IExpression Expression)
        {
            expression = Expression;
        }

        public void CheckIdentifiers(Scope Current)
        {
            expression.CheckIdentifiers(Current);
        }

        public MiniPascalType NodeType(Scope Current)
        {
            return expression.NodeType(Current);
        }

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            expression.EmitIR(Emitter);
        }
    }
}
