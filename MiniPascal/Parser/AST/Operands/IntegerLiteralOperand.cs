namespace MiniPL.Parser.AST
{
    public sealed class IntegerLiteralOperand : IOperand
    {
        private readonly int literal;

        public IntegerLiteralOperand(int Literal)
        {
            literal = Literal;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
        }

        public MiniPascalType NodeType(IdentifierTypes Types)
        {
            return MiniPascalType.Integer;
        }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.PushInt(literal);
        }

        public ReturnValue Execute(Variables Global)
        {
            return new ReturnValue(MiniPascalType.Integer, literal);
        }
    }
}
