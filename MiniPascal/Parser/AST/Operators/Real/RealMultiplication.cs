namespace MiniPascal.Parser.AST
{
    public sealed class RealMultiplication : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Real; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.Multiply();
        }
    }
}
