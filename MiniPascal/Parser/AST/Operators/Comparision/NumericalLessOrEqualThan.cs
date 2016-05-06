namespace MiniPascal.Parser.AST
{
    public sealed class NumericalLessOrEqualThan : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Boolean; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.GreaterThan();
            Emitter.PushInt32(0);
            Emitter.Equals();
        }
    }
}
