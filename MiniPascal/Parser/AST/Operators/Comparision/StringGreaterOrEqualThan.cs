namespace MiniPascal.Parser.AST
{
    public sealed class StringGreaterOrEqualThan : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Boolean; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.CallStringGreaterOrEqualThan();
        }
    }
}
