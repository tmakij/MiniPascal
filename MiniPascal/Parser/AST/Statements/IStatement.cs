namespace MiniPascal.Parser.AST
{
    public interface IStatement : IIdentifierHolder
    {
        void CheckType(Scope Current);
        void EmitIR(CILEmitter Emitter);
    }
}
