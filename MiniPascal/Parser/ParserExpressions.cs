﻿using MiniPascal.Lexer;
using MiniPascal.Parser.AST;
using System;
using System.Globalization;

namespace MiniPascal.Parser
{
    public sealed partial class Parser
    {
        private IExpression ReadExpression()
        {
            IExpression simpleExpr = ReadSimpleExpression();
            if (simpleExpr == null)
            {
                return null;
            }
            Expression<IExpression> expr = new Expression<IExpression>(simpleExpr);
            while (true)
            {
                OperatorType relational = RelationalOperator();
                if (relational == OperatorType.None)
                {
                    break;
                }
                IExpression addedExpr = ReadSimpleExpression();
                if (addedExpr == null)
                {
                    throw new SyntaxException("expression", current);
                }
                expr.Add(new OperatorPair<IExpression>(relational, addedExpr));
            }
            return expr;
        }

        private IExpression ReadSimpleExpression()
        {
            OperatorType sign = Sign();
            IExpression term = ReadTerm();
            if (term == null)
            {
                return null;
            }
            SignedExpression addExpr = new SignedExpression(sign, term);
            while (true)
            {
                OperatorType adding = AddingOperator();
                if (adding == OperatorType.None)
                {
                    break;
                }
                IExpression addedTerm = ReadTerm();
                if (addedTerm == null)
                {
                    throw new SyntaxException("term", current);
                }
                addExpr.Add(new OperatorPair<IExpression>(adding, addedTerm));
            }
            return addExpr;
        }

        private IExpression ReadTerm()
        {
            IOperand factor = Factor();
            if (factor == null)
            {
                return null;
            }
            Expression<IOperand> term = new Expression<IOperand>(factor);
            while (true)
            {
                OperatorType multiplication = MultiplyingOperator();
                if (multiplication == OperatorType.None)
                {
                    break;
                }
                IOperand addedFactor = Factor();
                if (addedFactor == null)
                {
                    throw new SyntaxException("factor", current);
                }
                term.Add(new OperatorPair<IOperand>(multiplication, addedFactor));
            }
            return term;
        }

        private IOperand Factor()
        {
            IOperand opr = VariableStartOpr() ?? ReadIntegerLiteral() ?? ReadRealLiteral()
                ?? ReadStringLiteral() ?? ReadBooleanLiteral() ?? Closures() ?? LogicalNot();
            if (opr == null)
            {
                return null;
            }
            if (Accept(Symbol.Period))
            {
                Require(Symbol.Size);
                return new Size(opr);
            }
            return opr;
        }

        private IOperand VariableStartOpr()
        {
            Identifier methodName = Identifier();
            if (methodName == null)
            {
                return null;
            }
            IOperand call = ReadCall(methodName);
            if (call != null)
            {
                return call;
            }
            return new VariableOperand(ReadVariableReference(methodName));
        }

        private IOperand ReadCall(Identifier Identifier)
        {

            if (Accept(Symbol.ClosureOpen))
            {
                Arguments args = ReadArguments();
                Require(Symbol.ClosureClose);
                return new Call(Identifier, args);
            }
            return null;
        }

        private IOperand ReadBooleanLiteral()
        {
            string lex;
            if (AcceptWithLexeme(Symbol.BooleanLiteral, out lex))
            {
                bool val = lex == "true";
                return new BooleanLiteral(val);
            }
            return null;
        }

        private IOperand ReadStringLiteral()
        {
            string lex;
            if (AcceptWithLexeme(Symbol.StringLiteral, out lex))
            {
                return new StringLiteralOperand(lex);
            }
            return null;
        }

        private IOperand ReadRealLiteral()
        {
            string lex;
            if (AcceptWithLexeme(Symbol.RealLiteral, out lex))
            {
                float val = float.Parse(lex, CultureInfo.InvariantCulture);
                return new RealLiteral(val);
            }
            return null;
        }

        private IOperand ReadIntegerLiteral()
        {
            string lex;
            if (AcceptWithLexeme(Symbol.IntegerLiteral, out lex))
            {
                int val = int.Parse(lex);
                return new IntegerLiteralOperand(val);
            }
            return null;
        }

        private IOperand Closures()
        {
            if (Accept(Symbol.ClosureOpen))
            {
                IExpression expr = ReadExpression();
                if (expr == null)
                {
                    throw new SyntaxException(expExpression, current);
                }
                Require(Symbol.ClosureClose);
                return new ExpressionOperand(expr);
            }
            return null;
        }

        private IOperand LogicalNot()
        {
            if (Accept(Symbol.LogicalNot))
            {
                IOperand factor = Factor();
                if (factor == null)
                {
                    throw new SyntaxException(expOperand, current);
                }
                return new Unary(OperatorType.Not, factor);
            }
            return null;
        }

        private OperatorType RelationalOperator()
        {
            if (Accept(Symbol.Equality))
            {
                return OperatorType.Equals;
            }
            if (Accept(Symbol.NotEquals))
            {
                return OperatorType.NotEquals;
            }
            if (Accept(Symbol.LessThan))
            {
                return OperatorType.LessThan;
            }
            if (Accept(Symbol.LessOrEqualThan))
            {
                return OperatorType.LessOrEqualThan;
            }
            if (Accept(Symbol.GreaterOrEqualThan))
            {
                return OperatorType.GreaterOrEqualThan;
            }
            if (Accept(Symbol.GreaterThan))
            {
                return OperatorType.GreaterThan;
            }
            return OperatorType.None;
        }

        private OperatorType Sign()
        {
            if (Accept(Symbol.Substraction))
            {
                return OperatorType.Substraction;
            }
            else if (Accept(Symbol.Addition))
            {
                return OperatorType.Addition;
            }
            return OperatorType.None;
        }

        private OperatorType AddingOperator()
        {
            if (Accept(Symbol.Substraction))
            {
                return OperatorType.Substraction;
            }
            if (Accept(Symbol.Addition))
            {
                return OperatorType.Addition;
            }
            if (Accept(Symbol.LogicalOr))
            {
                return OperatorType.Or;
            }
            return OperatorType.None;
        }

        private OperatorType MultiplyingOperator()
        {
            if (Accept(Symbol.Multiplication))
            {
                return OperatorType.Multiplication;
            }
            if (Accept(Symbol.Division))
            {
                return OperatorType.Division;
            }
            if (Accept(Symbol.Modulo))
            {
                return OperatorType.Modulo;
            }
            if (Accept(Symbol.LogicalAnd))
            {
                return OperatorType.And;
            }
            return OperatorType.None;
        }
    }
}
