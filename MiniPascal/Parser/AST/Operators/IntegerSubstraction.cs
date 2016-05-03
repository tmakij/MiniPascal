namespace MiniPascal.Parser.AST
{
    public sealed class IntegerSubstraction : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Integer; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.Substract();
        }
    }
}
