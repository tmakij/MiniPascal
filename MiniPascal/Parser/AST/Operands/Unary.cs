namespace MiniPascal.Parser.AST
{
    public sealed class Unary : IOperand
    {
        private readonly OperatorType oper;
        private readonly IOperand factor;

        public MiniPascalType Type { get { return factor.Type; } }

        public Unary(OperatorType Operator, IOperand Factor)
        {
            oper = Operator;
            factor = Factor;
        }

        public void CheckIdentifiers(Scope Current)
        {
            factor.CheckIdentifiers(Current);
        }

        public MiniPascalType NodeType(Scope Current)
        {
            factor.NodeType(Current);
            if (!Type.HasOperatorDefined(oper))
            {
                throw new UndefinedOperatorException(Type, oper);
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
            Type.UnaryOperation(oper).EmitIR(Emitter);
        }
    }
}
