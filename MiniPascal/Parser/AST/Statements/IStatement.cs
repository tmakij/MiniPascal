namespace MiniPL.Parser.AST
{
    public interface IStatement : IIdentifierHolder
    {
        void CheckType(IdentifierTypes Types);
        void Execute(Variables Scope);
    }
}
