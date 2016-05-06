using MiniPascal.Lexer;
using MiniPascal.Parser.AST;
using System;
using System.Globalization;

namespace MiniPascal.Parser
{
    public sealed partial class Parser
    {
        private Expression ReadExpression()
        {
            SimpleExpression simpleExpr = ReadSimpleExpression();
            if (simpleExpr == null)
            {
                return null;
            }
            Expression expr = new Expression(simpleExpr);
            while (true)
            {
                OperatorType relational = RelationalOperator();
                if (relational == OperatorType.None)
                {
                    break;
                }
                SimpleExpression addedExpr = ReadSimpleExpression();
                if (addedExpr == null)
                {
                    throw new SyntaxException("expression", symbol);
                }
                expr.Add(new OperatorPair<SimpleExpression>(relational, addedExpr));
            }
            return expr;
        }

        private SimpleExpression ReadSimpleExpression()
        {
            OperatorType sign = Sign();
            Term term = ReadTerm();
            if (term == null)
            {
                return null;
            }
            SimpleExpression addExpr = new SimpleExpression(sign, term);
            while (true)
            {
                OperatorType adding = AddingOperator();
                if (adding == OperatorType.None)
                {
                    break;
                }
                Term addedTerm = ReadTerm();
                if (addedTerm == null)
                {
                    throw new SyntaxException("term", symbol);
                }
                addExpr.Add(new OperatorPair<Term>(adding, addedTerm));
            }
            return addExpr;
        }

        private Term ReadTerm()
        {
            IOperand factor = Factor();
            if (factor == null)
            {
                return null;
            }
            Term term = new Term(factor);
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
                    throw new SyntaxException("factor", symbol);
                }
                term.Add(new OperatorPair<IOperand>(multiplication, addedFactor));
            }
            return term;
        }

        private IOperand Factor()
        {
            IOperand opr = ReadVariableOperand() ?? ReadIntegerLiteral() ?? ReadRealLiteral() ?? ReadStringLiteral() ?? ReadBooleanLiteral();
            return opr;
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

        private IOperand ReadVariableOperand()
        {
            string lex;
            if (AcceptWithLexeme(Symbol.Identifier, out lex))
            {
                return new VariableOperand(new Identifier(lex));
            }
            return null;
        }

        private IOperand ReadRealLiteral()
        {
            string lex;
            if (AcceptWithLexeme(Symbol.RealLiteral, out lex))
            {
                try
                {
                    float val = float.Parse(lex, CultureInfo.InvariantCulture);
                    return new RealLiteral(val);
                }
                catch (OverflowException)
                {
                    throw new IntegerParseOverflowException(lex);
                }
            }
            return null;
        }

        private IOperand ReadIntegerLiteral()
        {
            string lex;
            if (AcceptWithLexeme(Symbol.IntegerLiteral, out lex))
            {
                try
                {
                    int val = int.Parse(lex);
                    return new IntegerLiteralOperand(val);
                }
                catch (OverflowException)
                {
                    throw new IntegerParseOverflowException(lex);
                }
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
