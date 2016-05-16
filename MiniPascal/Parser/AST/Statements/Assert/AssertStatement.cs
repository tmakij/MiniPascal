namespace MiniPascal.Parser.AST
{
    public sealed class AssertStatement : IStatement
    {
        private readonly IExpression toAssert;

        public AssertStatement(IExpression ToAssert)
        {
            toAssert = ToAssert;
        }

        public void CheckIdentifiers(Scope Current)
        {
            toAssert.CheckIdentifiers(Current);
        }

        public void CheckType(Scope Current)
        {
            MiniPascalType type = toAssert.NodeType(Current);
            if (!type.Equals(MiniPascalType.Boolean))
            {
                throw new TypeMismatchException(MiniPascalType.Boolean, type);
            }
        }

        public void EmitIR(CILEmitter Emitter)
        {
            throw new System.NotImplementedException();
        }
    }
}
