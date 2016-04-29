namespace MiniPL.Parser.AST
{
    public sealed class StringConcatenation : IBinaryOperator
    {
        public MiniPLType ReturnType { get { return MiniPLType.String; } }

        public object Execute(object FirstOperand, object SecondOperand)
        {
            return FirstOperand.ToString() + SecondOperand.ToString();
        }
    }
}
