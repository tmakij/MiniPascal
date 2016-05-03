namespace MiniPascal.Parser.AST
{
    public sealed class StringEquals : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Boolean; } }

        public void EmitIR(CILEmitter Emitter)
        {
            throw new System.NotImplementedException();
        }
    }
}
