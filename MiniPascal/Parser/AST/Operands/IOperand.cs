namespace MiniPL.Parser.AST
{
    public interface IOperand : ITypedNode, IValuedExecutable
    {
        void EmitIR(CILEmitter Emitter);
    }
}
