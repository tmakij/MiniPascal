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
            MiniPLType type = toAssert.NodeType(Types);
            if (!type.Equals(MiniPLType.Boolean))
            {
                throw new TypeMismatchException(MiniPLType.Boolean, type);
            }
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
