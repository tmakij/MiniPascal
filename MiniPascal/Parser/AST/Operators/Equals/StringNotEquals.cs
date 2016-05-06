namespace MiniPascal.Parser.AST
{
    public sealed class StringNotEquals : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Boolean; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.CallStringEquals();
            Emitter.PushInt32(0);
            Emitter.Equals();
        }
    }
}
