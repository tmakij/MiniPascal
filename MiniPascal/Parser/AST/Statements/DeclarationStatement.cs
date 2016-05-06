namespace MiniPascal.Parser.AST
{
    public sealed class DeclarationStatement : IStatement
    {
        private readonly Identifier identifier;
        private readonly MiniPascalType type;

        public DeclarationStatement(Identifier Identifier, MiniPascalType Type, IExpression OptionalAssigment)
        {
            identifier = Identifier;
            type = Type;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            if (Used.IsUsed(identifier))
            {
                throw new VariableNameDefinedException(identifier);
            }
            Used.DeclareVariable(identifier);
        }

        public void CheckType(IdentifierTypes Types)
        {
            Types.SetIdentifierType(identifier, type);
        }

        public void EmitIR(CILEmitter Emitter, IdentifierTypes Types)
        {
            Emitter.CreateVariable(identifier, type);
            if (type.Equals(MiniPascalType.Integer))
            {
                Emitter.PushInt32(0);
            }
            else if (type.Equals(MiniPascalType.Real))
            {
                Emitter.PushSingle(0f);
            }
            else if (type.Equals(MiniPascalType.String))
            {
                Emitter.PushString("");
            }
            else if (type.Equals(MiniPascalType.Boolean))
            {
                Emitter.PushInt32(0);
            }
            Emitter.SaveVariable(identifier);
        }
    }
}
