namespace MiniPascal.Parser.AST
{
    public sealed class BooleanLiteral : IOperand
    {
        private readonly bool literal;

        public BooleanLiteral(bool Value)
        {
            literal = Value;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
        }

        public MiniPascalType NodeType(IdentifierTypes Types)
        {
            return MiniPascalType.Boolean;
        }

        public void EmitIR(CILEmitter Emitter)
        {
            int booleanValue = literal ? 1 : 0;
            Emitter.PushInt32(booleanValue);
        }
    }
}
