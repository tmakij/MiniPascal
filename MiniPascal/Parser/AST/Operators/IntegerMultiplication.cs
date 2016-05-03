namespace MiniPascal.Parser.AST
{
    public sealed class IntegerMultiplication : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Integer; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.Multiply();
        }
    }
}
