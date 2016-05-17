namespace MiniPascal.Parser.AST
{
    public sealed class RealDivision : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Real; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.Divide();
        }
    }
}
