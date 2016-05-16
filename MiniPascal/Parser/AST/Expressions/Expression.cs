﻿using System.Collections.Generic;

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

        public void CheckIdentifiers(Scope Current)
        {
            //System.Console.WriteLine("Expr ident");
            firstExpression.CheckIdentifiers(Current);
            foreach (OperatorPair<SimpleExpression> opr in simpleExpressions)
            {
                opr.Operand.CheckIdentifiers(Current);
            }
        }

        public MiniPascalType NodeType(Scope Current)
        {
            MiniPascalType type = firstExpression.NodeType(Current);
            foreach (OperatorPair<SimpleExpression> expr in simpleExpressions)
            {
                MiniPascalType anotherType = expr.Operand.NodeType(Current);
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

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            firstExpression.EmitIR(Emitter, Reference);
            foreach (OperatorPair<SimpleExpression> expr in simpleExpressions)
            {
                expr.Operand.EmitIR(Emitter, Reference);
                expr.Operand.Type.BinaryOperation(expr.Operator).EmitIR(Emitter);
            }
        }
    }
}
