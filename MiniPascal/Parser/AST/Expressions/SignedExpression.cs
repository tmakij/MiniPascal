namespace MiniPascal.Parser.AST
{
    public sealed class SignedExpression : Expression<IExpression>
    {
        private readonly OperatorType sign;

        public SignedExpression(OperatorType Sign, IExpression Term)
            : base(Term)
        {
            sign = Sign;
        }

        public override MiniPascalType NodeType(Scope Current)
        {
            base.NodeType(Current);
            if (sign != OperatorType.None && !Type.HasOperatorDefined(sign))
            {
                throw new UndefinedOperatorException(Type, sign);
            }
            return Type;
        }

        protected override void LoadFirst(CILEmitter Emitter, bool Reference)
        {
            base.LoadFirst(Emitter, Reference);
            if (sign == OperatorType.Substraction)
            {
                if (Type.Equals(MiniPascalType.Integer))
                {
                    Emitter.PushInt32(-1);
                }
                else if (Type.Equals(MiniPascalType.Real))
                {
                    Emitter.PushSingle(-1f);
                }
                Emitter.Multiply();
                return;
            }
        }
    }
}
