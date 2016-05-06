namespace MiniPascal.Parser.AST
{
    interface ILowerExpression : IExpression
    {
        MiniPascalType Type { get; }
    }
}
