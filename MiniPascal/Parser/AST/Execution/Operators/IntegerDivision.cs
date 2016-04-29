namespace MiniPL.Parser.AST
{
    public sealed class IntegerDivision : IBinaryOperator
    {
        public MiniPLType ReturnType { get { return MiniPLType.Integer; } }

        public object Execute(object FirstOperand, object SecondOperand)
        {
            int divident = (int)FirstOperand;
            int divisor = (int)SecondOperand;
            int res = divident / divisor;
            return res;
        }
    }
}
