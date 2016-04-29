namespace MiniPL.Parser.AST
{
    public sealed class StringConcatenation : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.String; } }

        public object Execute(object FirstOperand, object SecondOperand)
        {
            return FirstOperand.ToString() + SecondOperand.ToString();
        }
    }
}
