namespace MiniPL.Parser.AST
{
    public interface ITypedNode : IIdentifierHolder
    {
        MiniPLType NodeType(IdentifierTypes Types);
    }
}
