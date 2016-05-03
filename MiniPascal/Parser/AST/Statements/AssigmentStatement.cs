namespace MiniPascal.Parser.AST
{
    public sealed class AssigmentStatement : IStatement
    {
        private readonly Identifier identifier;
        private readonly Expression expression;

        public AssigmentStatement(Identifier Identifier, Expression Expression)
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
            MiniPascalType type = Types.GetIdentifierType(identifier);
            MiniPascalType assigmentType = expression.NodeType(Types);
            if (!assigmentType.Equals(type))
            {
                throw new TypeMismatchException(type, assigmentType);
            }
        }

        public void EmitIR(CILEmitter Emitter, IdentifierTypes Types)
        {
            expression.EmitIR(Emitter);
            Emitter.SaveVariable(identifier);
        }
    }
}
