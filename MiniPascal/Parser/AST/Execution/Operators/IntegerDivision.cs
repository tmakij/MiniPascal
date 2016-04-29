namespace MiniPL.Parser.AST
{
    public sealed class IntegerDivision : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Integer; } }

        public object Execute(object FirstOperand, object SecondOperand)
        {
            int divident = (int)FirstOperand;
            int divisor = (int)SecondOperand;
            int res = divident / divisor;
            return res;
        }
    }
}
