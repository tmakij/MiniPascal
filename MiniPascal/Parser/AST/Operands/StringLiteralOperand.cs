namespace MiniPascal.Parser.AST
{
    public sealed class StringLiteralOperand : IOperand
    {
        public MiniPascalType Type { get { return MiniPascalType.String; } }
        private readonly string literal;

        public StringLiteralOperand(string Literal)
        {
            literal = Literal;
        }

        public void CheckIdentifiers(Scope Current)
        {
        }

        public MiniPascalType NodeType(Scope Current)
        {
            return MiniPascalType.String;
        }

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            if (Reference)
            {
                throw new InvalidByReferenceException();
            }
            Emitter.PushString(literal);
        }
    }
}
