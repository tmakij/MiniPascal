namespace MiniPascal.Parser.AST
{
    public sealed class IntegerLiteralOperand : IOperand
    {
        public MiniPascalType Type { get { return MiniPascalType.Integer; } }
        private readonly int literal;

        public IntegerLiteralOperand(int Literal)
        {
            literal = Literal;
        }

        public void CheckIdentifiers(Scope Current)
        {
        }

        public MiniPascalType NodeType(Scope Current)
        {
            return MiniPascalType.Integer;
        }

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            Emitter.PushInt32(literal);
        }
    }
}
