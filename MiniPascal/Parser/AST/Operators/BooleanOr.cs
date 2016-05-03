namespace MiniPascal.Parser.AST
{
    public sealed class BooleanOr : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Boolean; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.Or();
        }
    }
}
