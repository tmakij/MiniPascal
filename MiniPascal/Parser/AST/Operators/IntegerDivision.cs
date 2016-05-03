namespace MiniPascal.Parser.AST
{
    public sealed class IntegerDivision : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Integer; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.Divide();
        }
    }
}
