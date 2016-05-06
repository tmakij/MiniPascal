namespace MiniPascal.Parser.AST
{
    public sealed class StringConcatenation : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.String; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.CallStringConcat();
        }
    }
}
