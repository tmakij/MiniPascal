namespace MiniPascal.Parser.AST
{
    public interface IIdentifierHolder
    {
        void CheckIdentifiers(Scope Current);
    }
}
