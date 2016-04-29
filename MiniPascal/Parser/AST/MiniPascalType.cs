using System;
using System.Collections.Generic;

namespace MiniPL.Parser.AST
{
    public sealed class MiniPascalType
    {
        public static MiniPascalType Integer { get; } = new MiniPascalType("Integer", default(int), typeof(int));
        public static MiniPascalType String { get; } = new MiniPascalType("String", string.Empty, typeof(string));
        public static MiniPascalType Boolean { get; } = new MiniPascalType("Boolean", default(bool), typeof(bool));
        public static MiniPascalType Real { get; } = new MiniPascalType("Real", default(float), typeof(float));

        static MiniPascalType()
        {
            Integer.AddBinaryOperator(OperatorType.Addition, new IntegerAddition());
            Integer.AddBinaryOperator(OperatorType.Multiplication, new IntegerMultiplication());
            Integer.AddBinaryOperator(OperatorType.Substraction, new IntegerSubstraction());
            Integer.AddBinaryOperator(OperatorType.Division, new IntegerDivision());
            Integer.AddBinaryOperator(OperatorType.Equals, new Equals<int>());
            Integer.AddBinaryOperator(OperatorType.LessThan, new LessThan<int>());
            String.AddBinaryOperator(OperatorType.Addition, new StringConcatenation());
            String.AddBinaryOperator(OperatorType.Equals, new Equals<string>());
            String.AddBinaryOperator(OperatorType.LessThan, new LessThan<string>());
            Boolean.AddUnaryOperator(OperatorType.Negation, new BooleanNegation());
            Boolean.AddBinaryOperator(OperatorType.Equals, new Equals<bool>());
            Boolean.AddBinaryOperator(OperatorType.LessThan, new LessThan<bool>());
            Boolean.AddBinaryOperator(OperatorType.And, new BooleanAnd());
        }

        public Type CLRType { get; }
        public object DefaultValue { get; }
        private readonly Dictionary<OperatorType, IBinaryOperator> binaryOperators = new Dictionary<OperatorType, IBinaryOperator>();
        private readonly Dictionary<OperatorType, IUnaryOperator> unaryOperators = new Dictionary<OperatorType, IUnaryOperator>();
        private readonly string name;

        private MiniPascalType(string Name, object DefaultValue, Type CLRType)
        {
            name = Name;
            this.DefaultValue = DefaultValue;
            this.CLRType = CLRType;
        }

        private void AddBinaryOperator(OperatorType Type, IBinaryOperator Operator)
        {
            binaryOperators.Add(Type, Operator);
        }

        private void AddUnaryOperator(OperatorType Type, IUnaryOperator Operator)
        {
            unaryOperators.Add(Type, Operator);
        }

        public IBinaryOperator BinaryOperation(OperatorType Operator)
        {
            return binaryOperators[Operator];
        }

        public object UnaryOperation(object Operand, OperatorType Operator)
        {
            return unaryOperators[Operator].Execute(Operand);
        }

        public bool HasOperatorDefined(OperatorType Operator)
        {
            return unaryOperators.ContainsKey(Operator) || binaryOperators.ContainsKey(Operator);
        }

        public override string ToString()
        {
            return name;
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            MiniPascalType other = (MiniPascalType)obj;
            return name == other.name;
        }
    }
}
