namespace MiniPascal.Parser.AST
{
    public sealed class BooleanLiteral : IOperand
    {
        private readonly bool literal;

        public BooleanLiteral(bool Value)
        {
            literal = Value;
        }

        public void CheckIdentifiers(Scope Current)
        {
        }

        public MiniPascalType NodeType(Scope Current)
        {
            return MiniPascalType.Boolean;
        }

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            int booleanValue = literal ? 1 : 0;
            Emitter.PushInt32(booleanValue);
        }
    }
}
