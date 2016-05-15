namespace MiniPascal.Parser.AST
{
    public interface IOperand : ITypedNode
    {
        void EmitIR(CILEmitter Emitter, bool Reference);
    }
}
