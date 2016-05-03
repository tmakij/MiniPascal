namespace MiniPascal.Parser.AST
{
    public interface ITypedNode : IIdentifierHolder
    {
        MiniPascalType NodeType(IdentifierTypes Types);
    }
}
