namespace MiniPascal.Parser.AST
{
    public interface IUnaryOperator
    {
        void EmitIR(CILEmitter Emitter);
    }
}
