namespace MiniPascal.Parser.AST
{
    public sealed class NumericalLessThan : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Boolean; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.LessThan();
        }
    }
}
