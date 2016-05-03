namespace MiniPascal.Parser.AST
{
    public interface IBinaryOperator
    {
        void EmitIR(CILEmitter Emitter);
        MiniPascalType ReturnType { get; }
    }
}
