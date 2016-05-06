namespace MiniPascal.Parser.AST
{
    public sealed class NumericalAddition : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Integer; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.Add();
        }
    }
}
