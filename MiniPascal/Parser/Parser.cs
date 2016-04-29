using MiniPL.Parser.AST;
using MiniPL.Lexer;
using System;
using System.Collections.Generic;

namespace MiniPL.Parser
{
    public sealed class Parser
    {
        private readonly TokenStream tokens;
        private Symbol symbol { get { return tokens.Symbol; } }

        private const string expExpression = "expression (operand-operator-operand or (unary operator)-operand)";
        private const string expOperand = "operand (variable, literal or expression between closures)";
        private const string expType = "int, string or bool";
        private const string expIdentifier = "identifier";

        public Parser(TokenStream Tokens)
        {
            tokens = Tokens;
        }

        public AbstractSyntaxTree Parse()
        {
            return Program();
        }

        private AbstractSyntaxTree Program()
        {
            Require(Symbol.Program);
            Require(Symbol.Identifier);
            Require(Symbol.SemiColon);
            ScopedProgram block = Block();
            Require(Symbol.Period);
            return new AbstractSyntaxTree(block);
        }

        private ScopedProgram Block()
        {
            Require(Symbol.Begin);
            ScopedProgram block = new ScopedProgram();

            while (true)
            {
                IStatement stm = Statement();
                if (stm == null)
                {
                    throw new SyntaxException("beginning of a statement", symbol);
                }
                block.Add(stm);
                if (!Accept(Symbol.SemiColon))
                {
                    Console.WriteLine("?? " + symbol);
                    Require(Symbol.End);
                    break;
                }
                if (Accept(Symbol.End))
                {
                    Console.WriteLine("YES");
                    break;
                }
            }

            return block;
        }

        private IStatement Statement()
        {
            return SimpleStatement() ?? StructuredStatement() ?? DeclarationStatement();
        }

        private IStatement SimpleStatement()
        {
            return AssigmentStatement() ?? CallStatement();
        }

        private IStatement AssigmentStatement()
        {
            Identifier ident = Identifier();
            if (ident != null)
            {
                Require(Symbol.Assigment);
                IExpression expr = Expression();
                if (expr == null)
                {
                    throw new SyntaxException(expExpression, symbol);
                }
                return new AssigmentStatement(ident, expr);
            }
            return null;
        }

        private IStatement CallStatement()
        {
            if (Accept(Symbol.PrintProcedure))
            {
                Require(Symbol.ClosureOpen);
                Identifier i = Identifier();
                //Require(Symbol.Identifier);
                Require(Symbol.ClosureClose);
                return new PrintStatement(new UnaryExpression(OperatorType.None, new VariableOperand(i)));
                //throw new NotImplementedException();
            }
            /*Identifier ident = Identifier();
            if (ident != null)
            {
            }*/
            return null;
        }

        private IStatement StructuredStatement()
        {
            return null;
        }

        private IStatement DeclarationStatement()
        {
            if (Accept(Symbol.Variable))
            {
                List<Identifier> ids = new List<Identifier>();
                do
                {
                    Identifier identifier = Identifier();
                    if (identifier == null)
                    {
                        throw new SyntaxException(expIdentifier, symbol);
                    }
                    ids.Add(identifier);
                } while (Accept(Symbol.Comma));

                Require(Symbol.Colon);
                MiniPLType type = Type();
                if (type == null)
                {
                    throw new SyntaxException(expType, symbol);
                }
                //Require(Symbol.SemiColon);
                return new DeclarationStatement(ids[0], type, null);
            }
            return null;
        }

        private IExpression Expression()
        {
            throw new NotImplementedException();
        }

        /*
        private IStatement Statement()
        {
            if (Accept(Symbol.Variable))
            {
                VariableIdentifier identifierToAssigment = Identifier();
                if (identifierToAssigment == null)
                {
                    throw new SyntaxException(expIdentifier, symbol);
                }
                Require(Symbol.Colon);
                MiniPLType type = Type();
                if (type == null)
                {
                    throw new SyntaxException(expType, symbol);
                }
                IExpression expr = null;
                if (Accept(Symbol.Assigment))
                {
                    expr = Expression();
                    if (expr == null)
                    {
                        throw new SyntaxException(expExpression, symbol);
                    }
                }
                return new DeclarationStatement(identifierToAssigment, type, expr);
            }

            VariableIdentifier varIdent = Identifier();
            if (varIdent != null)
            {
                Require(Symbol.Assigment);
                IExpression expr = Expression();
                if (expr == null)
                {
                    throw new SyntaxException(expExpression, symbol);
                }
                return new AssigmentStatement(varIdent, expr);
            }
            if (Accept(Symbol.ReadProcedure))
            {
                VariableIdentifier toReadInto = Identifier();
                if (toReadInto == null)
                {
                    throw new SyntaxException(expIdentifier, symbol);
                }
                return new ReadStatement(toReadInto);
            }
            if (Accept(Symbol.PrintProcedure))
            {
                IExpression toPrint = Expression();
                if (toPrint == null)
                {
                    throw new SyntaxException(expExpression, symbol);
                }
                return new PrintStatement(toPrint);
            }
            if (Accept(Symbol.Assert))
            {
                IExpression toAssert = Expression();
                if (toAssert == null)
                {
                    throw new SyntaxException(expExpression, symbol);
                }
                return new AssertStatement(toAssert);
            }
            return null;
        }

        private IExpression Expression()
        {
            if (Accept(Symbol.LogicalNot))
            {
                IOperand opr = Operand();
                if (opr == null)
                {
                    throw new SyntaxException(expOperand, symbol);
                }
                return new UnaryExpression(OperatorType.Negation, opr);
            }
            IOperand firstOperand = Operand();
            if (firstOperand != null)
            {
                OperatorType opr = ReadOperator();
                if (opr == OperatorType.None)
                {
                    return new UnaryExpression(OperatorType.None, firstOperand);
                }
                IOperand secondOperand = Operand();
                if (secondOperand == null)
                {
                    throw new SyntaxException(expOperand, symbol);
                }
                return new BinaryExpression(firstOperand, opr, secondOperand);
            }
            return null;
        }

        private OperatorType ReadOperator()
        {
            if (Accept(Symbol.Addition))
            {
                return OperatorType.Addition;
            }
            if (Accept(Symbol.Multiplication))
            {
                return OperatorType.Multiplication;
            }
            if (Accept(Symbol.Substraction))
            {
                return OperatorType.Substraction;
            }
            if (Accept(Symbol.Division))
            {
                return OperatorType.Division;
            }
            if (Accept(Symbol.Equality))
            {
                return OperatorType.Equals;
            }
            if (Accept(Symbol.LessThan))
            {
                return OperatorType.LessThan;
            }
            if (Accept(Symbol.LogicalAnd))
            {
                return OperatorType.And;
            }
            return OperatorType.None;
        }

        private IOperand Operand()
        {
            if (Matches(Symbol.IntegerLiteral))
            {
                try
                {
                    int val = int.Parse(tokens.Current.Lexeme);
                    tokens.Next();
                    return new IntegerLiteralOperand(val);
                }
                catch(OverflowException)
                {
                    throw new IntegerParseOverflowException(tokens.Current.Lexeme);
                }
            }
            if (Matches(Symbol.StringLiteral))
            {
                string val = tokens.Current.Lexeme;
                tokens.Next();
                return new StringLiteralOperand(val);
            }
            VariableIdentifier varIdent = Identifier();
            if (varIdent != null)
            {
                return new VariableOperand(varIdent);
            }
            if (Accept(Symbol.ClosureOpen))
            {
                IExpression expr = Expression();
                if (expr == null)
                {
                    throw new SyntaxException(expExpression, symbol);
                }
                Require(Symbol.ClosureClose);
                return new ExpressionOperand(expr);
            }
            return null;
        }

        private VariableIdentifier Identifier()
        {
            if (Matches(Symbol.Identifier))
            {
                VariableIdentifier id = new VariableIdentifier(tokens.Current.Lexeme);
                tokens.Next();
                return id;
            }
            return null;
        }
        */

        private MiniPLType Type()
        {
            if (Matches(Symbol.IntegerType))
            {
                tokens.Next();
                return MiniPLType.Integer;
            }
            if (Matches(Symbol.StringType))
            {
                tokens.Next();
                return MiniPLType.String;
            }
            if (Matches(Symbol.BooleanType))
            {
                tokens.Next();
                return MiniPLType.Boolean;
            }
            return null;
        }

        private Identifier Identifier()
        {
            if (Matches(Symbol.Identifier))
            {
                Identifier id = new Identifier(tokens.Current.Lexeme);
                tokens.Next();
                return id;
            }
            return null;
        }

        private bool Accept(Symbol Accepted)
        {
            if (Matches(Accepted))
            {
                tokens.Next();
                return true;
            }
            return false;
        }

        private bool Matches(Symbol Expected)
        {
            return symbol == Expected;
        }

        private void Require(Symbol Expected)
        {
            if (!Accept(Expected))
            {
                Symbol s = symbol;
                tokens.Next();
                throw new SyntaxException(Expected.ToString() + " " + tokens.Current.Symbol, s);
            }
        }
    }
}
