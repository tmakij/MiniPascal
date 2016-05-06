namespace MiniPascal.Parser.AST
{
    public sealed class NumericalNotEquals : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Boolean; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.Equals();
            Emitter.PushInt32(0);
            Emitter.Equals();
        }
    }
}
