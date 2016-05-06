namespace MiniPascal.Parser.AST
{
    public sealed class NumericalGreaterOrEqualThan : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Boolean; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.LessThan();
            Emitter.PushInt32(0);
            Emitter.Equals();
        }
    }
}
