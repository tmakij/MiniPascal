namespace MiniPascal.Parser.AST
{
    public sealed class RealLiteral : IOperand
    {
        public MiniPascalType Type { get { return MiniPascalType.Real; } }
        private readonly float literal;

        public RealLiteral(float Literal)
        {
            literal = Literal;
        }

        public void CheckIdentifiers(Scope Current)
        {
        }

        public MiniPascalType NodeType(Scope Current)
        {
            return MiniPascalType.Real;
        }

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            if (Reference)
            {
                throw new InvalidByReferenceException();
            }
            Emitter.PushSingle(literal);
        }
    }
}
