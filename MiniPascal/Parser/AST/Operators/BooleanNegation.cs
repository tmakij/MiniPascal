namespace MiniPascal.Parser.AST
{
    public sealed class BooleanNegation : IUnaryOperator
    {
        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.PushInt32(0);
            Emitter.Equals();
        }
    }
}
