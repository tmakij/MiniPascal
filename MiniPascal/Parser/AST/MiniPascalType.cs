using System;
using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class MiniPascalType : IEquatable<MiniPascalType>
    {
        public static MiniPascalType Integer { get; } = new MiniPascalType("Integer", typeof(int));
        public static MiniPascalType String { get; } = new MiniPascalType("String", typeof(string));
        public static MiniPascalType Boolean { get; } = new MiniPascalType("Boolean", typeof(bool));
        public static MiniPascalType Real { get; } = new MiniPascalType("Real", typeof(float));

        static MiniPascalType()
        {
            Integer.AddBinaryOperator(OperatorType.Addition, new IntegerAddition());
            Integer.AddBinaryOperator(OperatorType.Multiplication, new IntegerMultiplication());
            Integer.AddBinaryOperator(OperatorType.Substraction, new IntegerSubstraction());
            Integer.AddBinaryOperator(OperatorType.Division, new IntegerDivision());
            Integer.AddBinaryOperator(OperatorType.Equals, new NumericalEquals());
            Integer.AddBinaryOperator(OperatorType.LessThan, new LessThan<int>());
            Integer.AddBinaryOperator(OperatorType.Modulo, new IntegerModulo());

            Real.AddBinaryOperator(OperatorType.Addition, new IntegerAddition());
            Real.AddBinaryOperator(OperatorType.Multiplication, new IntegerMultiplication());
            Real.AddBinaryOperator(OperatorType.Substraction, new IntegerSubstraction());
            Real.AddBinaryOperator(OperatorType.Division, new IntegerDivision());
            Real.AddBinaryOperator(OperatorType.Equals, new NumericalEquals());

            String.AddBinaryOperator(OperatorType.Addition, new StringConcatenation());
            String.AddBinaryOperator(OperatorType.Equals, new StringEquals());
            String.AddBinaryOperator(OperatorType.LessThan, new LessThan<string>());

            Boolean.AddUnaryOperator(OperatorType.Negation, new BooleanNegation());
            Boolean.AddBinaryOperator(OperatorType.Equals, new NumericalEquals());
            Boolean.AddBinaryOperator(OperatorType.LessThan, new LessThan<bool>());
            Boolean.AddBinaryOperator(OperatorType.And, new BooleanAnd());
        }

        public Type[] CLRTypeArray { get; }
        public Type CLRType { get { return CLRTypeArray[0]; } }
        private readonly Dictionary<OperatorType, IBinaryOperator> binaryOperators = new Dictionary<OperatorType, IBinaryOperator>();
        private readonly Dictionary<OperatorType, IUnaryOperator> unaryOperators = new Dictionary<OperatorType, IUnaryOperator>();
        private readonly string name;

        private MiniPascalType(string Name, Type CLRType)
        {
            name = Name;
            CLRTypeArray = new[] { CLRType };
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

        public IUnaryOperator UnaryOperation(OperatorType Operator)
        {
            return unaryOperators[Operator];
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
            return Equals((MiniPascalType)obj);
        }

        public bool Equals(MiniPascalType other)
        {
            return name == other.name;
        }
    }
}
