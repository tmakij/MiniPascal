namespace MiniPL.Parser.AST
{
    public interface IBinaryOperator
    {
        object Execute(object FirstOperand, object SecondOperand);
        MiniPascalType ReturnType { get; }
    }
}
