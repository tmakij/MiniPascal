namespace MiniPascal.Parser.AST
{
    public sealed class BooleanNegation : IUnaryOperator
    {
        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.Negate();
        }
    }
}
