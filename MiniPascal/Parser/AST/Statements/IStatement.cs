namespace MiniPascal.Parser.AST
{
    public interface IStatement : IIdentifierHolder
    {
        void CheckType(IdentifierTypes Types);
        void EmitIR(CILEmitter Emitter, IdentifierTypes Types);
    }
}
