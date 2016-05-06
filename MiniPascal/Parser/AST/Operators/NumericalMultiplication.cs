namespace MiniPascal.Parser.AST
{
    public sealed class NumericalMultiplication : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Integer; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.Multiply();
        }
    }
}
