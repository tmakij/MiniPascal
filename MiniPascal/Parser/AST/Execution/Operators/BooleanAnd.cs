namespace MiniPL.Parser.AST
{
    public sealed class BooleanAnd : IBinaryOperator
    {
        public object Execute(object FirstOperand, object SecondOperand)
        {
            bool first = (bool)FirstOperand;
            bool second = (bool)SecondOperand;
            return first && second;
        }

        public MiniPascalType ReturnType { get { return MiniPascalType.Boolean; } }
    }
}
