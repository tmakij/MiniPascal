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

        public void CheckIdentifiers(Scope Current)
        {
            if (!Current.IsUsed(identifier))
            {
                throw new UninitializedVariableException(identifier);
            }
            variable = Current.Variable(identifier);
        }

        public MiniPascalType NodeType(Scope Current)
        {
            return Current.Variable(identifier).Type;
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
