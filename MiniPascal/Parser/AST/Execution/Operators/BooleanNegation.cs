namespace MiniPL.Parser.AST
{
    public sealed class BooleanNegation : IUnaryOperator
    {
        public object Execute(object Operand)
        {
            bool val = (bool)Operand;
            return !val;
        }
    }
}
