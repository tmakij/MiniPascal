using System;

namespace MiniPL.Parser.AST
{
    public sealed class Equals<T> : IBinaryOperator where T : IEquatable<T>
    {
        public MiniPLType ReturnType { get { return MiniPLType.Boolean; } }

        public object Execute(object FirstOperand, object SecondOperand)
        {
            T val1 = (T)FirstOperand;
            T val2 = (T)SecondOperand;
            bool res = val1.Equals(val2);
            return res;
        }
    }
}
