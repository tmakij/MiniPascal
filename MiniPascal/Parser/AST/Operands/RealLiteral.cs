namespace MiniPL.Parser.AST
{
    public sealed class RealLiteral : IOperand
    {
        private readonly float literal;

        public RealLiteral(float Literal)
        {
            literal = Literal;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
        }

        public MiniPascalType NodeType(IdentifierTypes Types)
        {
            return MiniPascalType.Real;
        }

        public ReturnValue Execute(Variables Global)
        {
            return new ReturnValue(MiniPascalType.Real, literal);
        }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.PushSingle(literal);
        }
    }
}
