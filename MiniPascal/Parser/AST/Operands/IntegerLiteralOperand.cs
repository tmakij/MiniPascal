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

        public MiniPLType NodeType(IdentifierTypes Types)
        {
            return MiniPLType.Integer;
        }

        public ReturnValue Execute(Variables Global)
        {
            return new ReturnValue(MiniPLType.Integer, literal);
        }
    }
}
