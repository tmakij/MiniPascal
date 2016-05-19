using System;

namespace MiniPascal.Parser.AST
{
    public sealed class MiniPascalType : IEquatable<MiniPascalType>
    {
        public static MiniPascalType Boolean { get; } = new MiniPascalType(SimpleType.Boolean, null, false);
        public static MiniPascalType Real { get; } = new MiniPascalType(SimpleType.Real, null, false);
        public static MiniPascalType Integer { get; } = new MiniPascalType(SimpleType.Integer, null, false);
        public static MiniPascalType String { get; } = new MiniPascalType(SimpleType.String, null, false);

        public SimpleType SimpleType { get; }
        public IExpression Size { get; }
        public bool IsArray { get; }

        public MiniPascalType(SimpleType Type, IExpression ArraySize)
            : this(Type, ArraySize, true)
        {
        }

        private MiniPascalType(SimpleType Type, IExpression ArraySize, bool Array)
        {
            SimpleType = Type;
            Size = ArraySize;
            IsArray = Array;
        }

        public bool HasOperatorDefined(OperatorType Operator)
        {
            if (IsArray)
            {
                return false;
            }
            return SimpleType.HasOperatorDefined(Operator);
        }

        public IBinaryOperator BinaryOperation(OperatorType Operator)
        {
            return SimpleType.BinaryOperation(Operator);
        }

        public IUnaryOperator UnaryOperation(OperatorType Operator)
        {
            return SimpleType.UnaryOperation(Operator);
        }

        public override int GetHashCode()
        {
            return SimpleType.GetHashCode() * (IsArray ? 7 : 13);
        }

        public bool Equals(MiniPascalType Other)
        {
            return IsArray == Other.IsArray && SimpleType.Equals(Other.SimpleType);
        }

        public override bool Equals(object obj)
        {
            return Equals((MiniPascalType)obj);
        }

        public override string ToString()
        {
            if (IsArray)
            {
                return "Array of " + SimpleType;
            }
            return SimpleType.ToString();
        }
    }
}
