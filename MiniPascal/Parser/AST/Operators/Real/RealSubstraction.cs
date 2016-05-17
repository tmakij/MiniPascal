namespace MiniPascal.Parser.AST
{
    public sealed class RealSubstraction : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Real; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.Substract();
        }
    }
}
