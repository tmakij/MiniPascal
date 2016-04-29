namespace MiniPL.Parser.AST
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

        public ReturnValue Execute(Variables Global)
        {
            RuntimeVariable var = Global.GetValue(variable);
            return new ReturnValue(var.Type, var.Value);
        }
    }
}
