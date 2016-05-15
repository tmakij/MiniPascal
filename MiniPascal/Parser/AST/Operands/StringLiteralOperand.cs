namespace MiniPascal.Parser.AST
{
    public sealed class StringLiteralOperand : IOperand
    {
        private readonly string literal;

        public StringLiteralOperand(string Literal)
        {
            literal = Literal;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
        }

        public MiniPascalType NodeType(IdentifierTypes Types)
        {
            return MiniPascalType.String;
        }

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            Emitter.PushString(literal);
        }
    }
}
