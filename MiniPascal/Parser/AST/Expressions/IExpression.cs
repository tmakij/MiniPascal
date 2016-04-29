namespace MiniPL.Parser.AST
{
    public interface IExpression : ITypedNode, IValuedExecutable
    {
        void EmitIR(CILEmitter Emitter);
    }
}
