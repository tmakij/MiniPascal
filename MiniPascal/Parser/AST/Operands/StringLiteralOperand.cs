namespace MiniPL.Parser.AST
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

        public MiniPLType NodeType(IdentifierTypes Types)
        {
            return MiniPLType.String;
        }

        public ReturnValue Execute(Variables Global)
        {
            return new ReturnValue(MiniPLType.String, literal);
        }
    }
}
