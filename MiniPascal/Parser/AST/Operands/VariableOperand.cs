namespace MiniPascal.Parser.AST
{
    public sealed class VariableOperand : IOperand
    {
        private readonly Identifier identifier;
        private Variable variable;

        public VariableOperand(Identifier Variable)
        {
            identifier = Variable;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            if (!Used.IsUsed(identifier))
            {
                throw new UninitializedVariableException(identifier);
            }
            variable = Used.Variable(identifier);
        }

        public MiniPascalType NodeType(IdentifierTypes Types)
        {
            return Types.GetIdentifierType(identifier);
        }

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            if (Reference)
            {
                Emitter.LoadVariableAddress(identifier);
            }
            else if (variable.IsReference)
            {
                Emitter.LoadReferenceVariable(variable);
            }
            else
            {
                Emitter.LoadVariable(identifier);
            }
        }
    }
}
