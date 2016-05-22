namespace MiniPascal.Parser.AST
{
    public sealed class Size : IOperand
    {
        private readonly IOperand factor;

        public MiniPascalType Type { get { return MiniPascalType.Integer; } }

        public Size(IOperand Factor)
        {
            factor = Factor;
        }

        public void CheckIdentifiers(Scope Current)
        {
            factor.CheckIdentifiers(Current);
        }

        public MiniPascalType NodeType(Scope Current)
        {
            factor.NodeType(Current);
            if (!factor.Type.IsArray)
            {
                throw new ArrayRequiredException(Type);
            }
            return Type;
        }

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            if (Reference)
            {
                throw new InvalidByReferenceException();
            }
            factor.EmitIR(Emitter, false);
            Emitter.ArraySize();
            Emitter.ToInt32();
        }
    }
}
