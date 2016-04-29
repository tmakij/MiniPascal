namespace MiniPL.Parser.AST
{
    public sealed class AssertStatement : IStatement
    {
        private readonly IExpression toAssert;

        public AssertStatement(IExpression ToAssert)
        {
            toAssert = ToAssert;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            toAssert.CheckIdentifiers(Used);
        }

        public void CheckType(IdentifierTypes Types)
        {
            MiniPascalType type = toAssert.NodeType(Types);
            if (!type.Equals(MiniPascalType.Boolean))
            {
                throw new TypeMismatchException(MiniPascalType.Boolean, type);
            }
        }

        public void EmitIR(CILEmitter Emitter)
        {
            throw new System.NotImplementedException();
        }

        public void Execute(Variables Scope)
        {
            ReturnValue ret = toAssert.Execute(Scope);
            bool assert = (bool)ret.Value;
            if (!assert)
            {
                throw new AssertationExecption();
            }
        }
    }
}
