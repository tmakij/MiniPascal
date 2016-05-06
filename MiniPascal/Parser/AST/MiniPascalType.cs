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
            NumericalAddition add = new NumericalAddition();
            NumericalMultiplication mul = new NumericalMultiplication();
            NumericalSubstraction sub = new NumericalSubstraction();
            NumericalDivision div = new NumericalDivision();

            NumericalEquals eq = new NumericalEquals();
            NumericalNotEquals noteq = new NumericalNotEquals();

            NumericalGreaterOrEqualThan greq = new NumericalGreaterOrEqualThan();
            NumericalLessOrEqualThan leeq = new NumericalLessOrEqualThan();
            NumericalGreaterThan gre = new NumericalGreaterThan();
            NumericalLessThan leq = new NumericalLessThan();

            Integer.AddBinaryOperator(OperatorType.Addition, add);
            Integer.AddBinaryOperator(OperatorType.Multiplication, mul);
            Integer.AddBinaryOperator(OperatorType.Substraction, sub);
            Integer.AddBinaryOperator(OperatorType.Division, div);

            Integer.AddBinaryOperator(OperatorType.Equals, eq);
            Integer.AddBinaryOperator(OperatorType.NotEquals, noteq);
            Integer.AddBinaryOperator(OperatorType.Modulo, new IntegerModulo());

            Integer.AddBinaryOperator(OperatorType.GreaterOrEqualThan, greq);
            Integer.AddBinaryOperator(OperatorType.GreaterThan, gre);
            Integer.AddBinaryOperator(OperatorType.LessOrEqualThan, leeq);
            Integer.AddBinaryOperator(OperatorType.LessThan, leq);

            Real.AddBinaryOperator(OperatorType.Addition, add);
            Real.AddBinaryOperator(OperatorType.Multiplication, mul);
            Real.AddBinaryOperator(OperatorType.Substraction, sub);
            Real.AddBinaryOperator(OperatorType.Division, div);

            Real.AddBinaryOperator(OperatorType.Equals, eq);
            Real.AddBinaryOperator(OperatorType.NotEquals, noteq);

            Real.AddBinaryOperator(OperatorType.GreaterOrEqualThan, greq);
            Real.AddBinaryOperator(OperatorType.GreaterThan, gre);
            Real.AddBinaryOperator(OperatorType.LessOrEqualThan, leeq);
            Real.AddBinaryOperator(OperatorType.LessThan, leq);

            String.AddBinaryOperator(OperatorType.Addition, new StringConcatenation());
            String.AddBinaryOperator(OperatorType.Equals, new StringEquals());
            String.AddBinaryOperator(OperatorType.NotEquals, new StringNotEquals());
            String.AddBinaryOperator(OperatorType.LessThan, new StringLessThan());

            Boolean.AddUnaryOperator(OperatorType.Negation, new BooleanNegation());
            Boolean.AddBinaryOperator(OperatorType.And, new BooleanAnd());
            Boolean.AddBinaryOperator(OperatorType.Or, new BooleanOr());

            Boolean.AddBinaryOperator(OperatorType.Equals, eq);
            Boolean.AddBinaryOperator(OperatorType.NotEquals, noteq);

            Boolean.AddBinaryOperator(OperatorType.GreaterOrEqualThan, greq);
            Boolean.AddBinaryOperator(OperatorType.GreaterThan, gre);
            Boolean.AddBinaryOperator(OperatorType.LessOrEqualThan, leeq);
            Boolean.AddBinaryOperator(OperatorType.LessThan, leq);
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
