namespace MiniPascal.Parser.AST
{
    public interface ITypedNode : IIdentifierHolder
    {
        MiniPascalType Type { get; }
        MiniPascalType NodeType(Scope Current);
        void EmitIR(CILEmitter Emitter, bool Reference);
    }
}
