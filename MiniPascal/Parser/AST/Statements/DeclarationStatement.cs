namespace MiniPL.Parser.AST
{
    public sealed class DeclarationStatement : IStatement
    {
        private readonly Identifier identifier;
        private readonly MiniPLType type;
        private readonly IExpression optionalAssigment;

        public DeclarationStatement(Identifier Identifier, MiniPLType Type, IExpression OptionalAssigment)
        {
            identifier = Identifier;
            type = Type;
            optionalAssigment = OptionalAssigment;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            if (Used.IsUsed(identifier))
            {
                throw new VariableNameDefinedException(identifier);
            }
            Used.DeclareVariable(identifier);
            if (optionalAssigment != null)
            {
                optionalAssigment.CheckIdentifiers(Used);
            }
        }

        public void CheckType(IdentifierTypes Types)
        {
            Types.SetIdentifierType(identifier, type);
            if (optionalAssigment != null)
            {
                MiniPLType assigmentType = optionalAssigment.NodeType(Types);
                if (!assigmentType.Equals(type))
                {
                    throw new TypeMismatchException(type, assigmentType);
                }
            }
        }

        public void Execute(Variables Scope)
        {
            Scope.AddIdentifier(identifier, type);
            if (optionalAssigment != null)
            {
                object assigment = optionalAssigment.Execute(Scope).Value;
                Scope.GetValue(identifier).Value = assigment;
            }
        }
    }
}
