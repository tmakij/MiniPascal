namespace MiniPascal.Parser.AST
{
    public sealed class VariableOperand : IOperand
    {
        private readonly Identifier variable;

        public VariableOperand(Identifier Variable)
        {
            variable = Variable;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            if (!Used.IsUsed(variable))
            {
                throw new UninitializedVariableException(variable);
            }
        }

        public MiniPascalType NodeType(IdentifierTypes Types)
        {
            return Types.GetIdentifierType(variable);
        }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.LoadVariable(variable);
        }
    }
}
