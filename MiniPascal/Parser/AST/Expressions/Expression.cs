using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public class Expression
    {
        private readonly List<OperatorPair<SimpleExpression>> simpleExpressions = new List<OperatorPair<SimpleExpression>>();
        private readonly SimpleExpression firstExpression;

        public Expression(SimpleExpression FirstExpression)
        {
            firstExpression = FirstExpression;
        }

        public void Add(OperatorPair<SimpleExpression> SimpleExpression)
        {
            simpleExpressions.Add(SimpleExpression);
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            System.Console.WriteLine("Expr ident");
            firstExpression.CheckIdentifiers(Used);
            foreach (OperatorPair<SimpleExpression> opr in simpleExpressions)
            {
                opr.Operand.CheckIdentifiers(Used);
            }
        }

        public MiniPascalType NodeType(IdentifierTypes Types)
        {
            MiniPascalType type = firstExpression.NodeType(Types);
            foreach (OperatorPair<SimpleExpression> expr in simpleExpressions)
            {
                MiniPascalType anotherType = expr.Operand.NodeType(Types);
                if (!type.Equals(anotherType))
                {
                    throw new InvalidTypeException(anotherType, type);
                }
                if (!type.HasOperatorDefined(expr.Operator))
                {
                    throw new UndefinedOperatorException(type, expr.Operator);
                }
                type = anotherType.BinaryOperation(expr.Operator).ReturnType;
                /*if (!type.Equals(operationResultType))
                {
                    throw new InvalidTypeException(operationResultType, type);
                }*/
            }
            return type;
        }

        public void EmitIR(CILEmitter Emitter)
        {
            firstExpression.EmitIR(Emitter);
            foreach (OperatorPair<SimpleExpression> expr in simpleExpressions)
            {
                expr.Operand.EmitIR(Emitter);
                expr.Operand.Type.BinaryOperation(expr.Operator).EmitIR(Emitter);
            }
        }
    }
}
