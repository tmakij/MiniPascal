namespace MiniPascal.Parser.AST
{
    public interface IExpression : ITypedNode
    {
        void EmitIR(CILEmitter Emitter);
    }
}
