namespace MiniPascal.Parser.AST
{
    public sealed class VariableReference
    {
        public Identifier Name { get; }
        public IExpression ArrayIndex { get; }

        public VariableReference(Identifier Identifier, IExpression IntegerExpression)
        {
            Name = Identifier;
            ArrayIndex = IntegerExpression;
        }
    }
}
