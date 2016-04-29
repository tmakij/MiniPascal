namespace MiniPL.Parser.AST
{
    public sealed class IntegerAddition : IBinaryOperator
    {
        public MiniPLType ReturnType { get { return MiniPLType.Integer; } }

        public object Execute(object FirstOperand, object SecondOperand)
        {
            int val1 = (int)FirstOperand;
            int val2 = (int)SecondOperand;
            int res = val1 + val2;
            return res;
        }
    }
}
