namespace MiniPL.Parser.AST
{
    public interface IBinaryOperator
    {
        object Execute(object FirstOperand, object SecondOperand);
        MiniPLType ReturnType { get; }
    }
}
