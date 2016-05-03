using MiniPascal.Lexer;
using MiniPascal.Parser.AST;
using System.Collections.Generic;

namespace MiniPascal.Parser
{
    public sealed partial class Parser
    {
        private IStatement Statement()
        {
            return SimpleStatement() ?? StructuredStatement() ?? DeclarationStatement(VarRequired.Yes);
        }

        private IStatement SimpleStatement()
        {
            return AssigmentStatement() ?? CallStatement() ?? ReturnStatement();
        }

        private IStatement AssigmentStatement()
        {
            Identifier ident = Identifier();
            if (ident != null)
            {
                Require(Symbol.Assigment);
                Expression expr = ReadExpression();
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
                Arguments args = ReadArguments();
                Require(Symbol.ClosureClose);
                return new PrintStatement(args);
                //return new PrintStatement(new UnaryExpression(OperatorType.None, new VariableOperand(i)));
                //throw new NotImplementedException();
            }
            /*Identifier ident = Identifier();
            if (ident != null)
            {
            }*/
            return null;
        }

        private IStatement ReturnStatement()
        {
            if (Accept(Symbol.Return))
            {
                Expression expr = ReadExpression();
                if (expr == null)
                {
                    throw new SyntaxException(expExpression, symbol);
                }
                throw new System.NotImplementedException();
            }
            return null;
        }

        private IStatement StructuredStatement()
        {
            return null;
        }

        private DeclarationStatement DeclarationStatement(VarRequired Required)
        {
            if (Accept(Symbol.Variable) || Required == VarRequired.No)
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
                MiniPascalType type = Type();
                if (type == null)
                {
                    throw new SyntaxException(expType, symbol);
                }
                //Require(Symbol.SemiColon);
                return new DeclarationStatement(ids[0], type, null);
            }
            return null;
        }

        private Arguments ReadArguments()
        {
            Arguments args = new Arguments();
            do
            {
                Expression ident = ReadExpression();
                if (ident == null)
                {
                    throw new SyntaxException(expExpression, symbol);
                }
                args.Add(ident);
            } while (Accept(Symbol.Comma));

            return args;
        }

        private Parameters ReadParameters()
        {
            Parameters parameters = new Parameters();
            do
            {
                DeclarationStatement decl = DeclarationStatement(VarRequired.No);
                parameters.Add(decl);
            } while (Accept(Symbol.Comma));
            return parameters;
        }
    }
}
