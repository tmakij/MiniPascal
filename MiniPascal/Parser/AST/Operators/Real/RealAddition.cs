namespace MiniPascal.Parser.AST
{
    public sealed class RealAddition : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Real; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.Add();
        }
    }
}
