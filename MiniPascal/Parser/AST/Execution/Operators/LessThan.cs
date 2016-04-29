using System;

namespace MiniPL.Parser.AST
{
    public sealed class LessThan<T> : IBinaryOperator where T : IComparable<T>
    {
        public MiniPLType ReturnType { get { return MiniPLType.Boolean; } }

        public object Execute(object FirstOperand, object SecondOperand)
        {
            T val1 = (T)FirstOperand;
            T val2 = (T)SecondOperand;
            int compRes = val1.CompareTo(val2);
            bool less = compRes < 0;
            return less;
        }
    }
}
