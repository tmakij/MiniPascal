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
            /*
            SimpleType type = toAssert.NodeType(Current);
            if (!type.Equals(SimpleType.Boolean))
            {
                throw new TypeMismatchException(SimpleType.Boolean, type);
            }
            */
        }

        public void EmitIR(CILEmitter Emitter)
        {
            throw new System.NotImplementedException();
        }
    }
}
