namespace MiniPascal.Parser.AST
{
    public sealed class StringGreaterThan : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Boolean; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.CallStringGreaterThan();
        }
    }
}
