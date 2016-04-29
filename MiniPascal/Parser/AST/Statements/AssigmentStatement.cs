namespace MiniPL.Parser.AST
{
    public sealed class AssigmentStatement : IStatement
    {
        private readonly Identifier identifier;
        private readonly IExpression expression;
        private MiniPascalType type;

        public AssigmentStatement(Identifier Identifier, IExpression Expression)
        {
            identifier = Identifier;
            expression = Expression;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            if (!Used.IsUsed(identifier))
            {
                throw new UninitializedVariableException(identifier);
            }
            if (!Used.IsMutable(identifier))
            {
                throw new ImmutableVariableException(identifier);
            }
            expression.CheckIdentifiers(Used);
        }

        public void CheckType(IdentifierTypes Types)
        {
            type = Types.GetIdentifierType(identifier);
            MiniPascalType assigmentType = expression.NodeType(Types);
            if (!assigmentType.Equals(type))
            {
                throw new TypeMismatchException(type, assigmentType);
            }
        }

        public void EmitIR(CILEmitter Emitter)
        {
            expression.EmitIR(Emitter);
            Emitter.SaveVariable(identifier);
        }

        public void Execute(Variables Scope)
        {
            RuntimeVariable var = Scope.GetValue(identifier);
            ReturnValue ret = expression.Execute(Scope);
            var.Value = ret.Value;
        }
    }
}
