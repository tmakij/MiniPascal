namespace MiniPascal.Parser.AST
{
    public sealed class NumericalDivision : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Integer; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.Divide();
        }
    }
}
