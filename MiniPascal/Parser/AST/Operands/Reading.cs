namespace MiniPascal.Parser.AST
{
    public sealed class Reading : IOperand
    {
        public MiniPascalType Type { get { return variable.Type; } }
        private readonly Identifier variableName;
        private Variable variable;

        public Reading(Identifier VariableName)
        {
            variableName = VariableName;
        }


        public void CheckIdentifiers(Scope Current)
        {
            if (!Current.IsUsed(variableName))
            {
                throw new UninitializedVariableException(variableName);
            }
            variable = Current.Variable(variableName);
        }

        public MiniPascalType NodeType(Scope Current)
        {
            return Type;
        }

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            if (Reference)
            {
                throw new InvalidByReferenceException();
            }
            Emitter.CallRead(Type);
        }
    }
}
