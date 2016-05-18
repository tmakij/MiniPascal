using MiniPascal.Lexer;
using MiniPascal.Parser.AST;
using System.Collections.Generic;

namespace MiniPascal.Parser
{
    public sealed partial class Parser
    {
        private IStatement Statement()
        {
            return SimpleStatement() ?? StructuredStatement() ?? DeclarationStatement();
        }

        private IStatement SimpleStatement()
        {
            return IdentifierStart() ?? CallPrint() ?? ReturnStatement();
        }

        private IStatement IdentifierStart()
        {
            Identifier ident = Identifier();
            if (ident != null)
            {
                IStatement call = CallStatement(ident);
                if (call != null)
                {
                    return call;
                }
                return AssigmentStatement(ident);
            }
            return null;
        }

        private IStatement AssigmentStatement(Identifier Identifier)
        {
            VariableReference varRef = ReadVariableReference(Identifier);
            Require(Symbol.Assigment);
            IExpression expr = ReadExpression();
            if (expr == null)
            {
                throw new SyntaxException(expExpression, current);
            }
            return new AssigmentStatement(varRef, expr);
        }

        private IStatement CallStatement(Identifier Identifier)
        {
            if (Accept(Symbol.ClosureOpen))
            {
                Arguments args = ReadArguments();
                Require(Symbol.ClosureClose);
                return new NoReturnCall(new Call(Identifier, args));
            }
            return null;
        }

        private IStatement CallPrint()
        {
            if (Accept(Symbol.PrintProcedure))
            {
                Require(Symbol.ClosureOpen);
                Arguments args = ReadArguments();
                Require(Symbol.ClosureClose);
                return new PrintStatement(args);
            }
            return null;
        }

        private IStatement ReturnStatement()
        {
            if (Accept(Symbol.Return))
            {
                IExpression expr = ReadExpression();
                return new Return(expr);
            }
            return null;
        }

        private IStatement StructuredStatement()
        {
            ScopedProgram block = Block();
            if (block != null)
            {
                return block;
            }
            If ifStm = IfStatement();
            if (ifStm != null)
            {
                return ifStm;
            }
            return null;
        }

        private If IfStatement()
        {
            if (Accept(Symbol.If))
            {
                Require(Symbol.ClosureOpen);
                IExpression condition = ReadExpression();
                Require(Symbol.ClosureClose);
                if (condition == null)
                {
                    throw new SyntaxException(expExpression, current);
                }
                Require(Symbol.Then);
                IStatement then = Statement();
                if (then == null)
                {
                    throw new SyntaxException("statement", current);
                }
                IStatement elseStatement;
                if (Accept(Symbol.Else))
                {
                    elseStatement = Statement();
                    if (elseStatement == null)
                    {
                        throw new SyntaxException("statement", current);
                    }
                }
                else
                {
                    elseStatement = null;
                }
                return new If(condition, then, elseStatement);
            }
            return null;
        }

        private IStatement DeclarationStatement()
        {
            DeclarationStatement varDecl = VariableDeclaration();
            if (varDecl != null)
            {
                return varDecl;
            }
            Procedure procDecl = ProcedureStatement();
            if (procDecl != null)
            {
                return procDecl;
            }
            return ReadFunction();
        }

        private Procedure ProcedureStatement()
        {
            if (Accept(Symbol.Procedure))
            {
                Identifier ident = Identifier();
                Require(Symbol.ClosureOpen);
                Parameters parameters = ReadParameters();
                Require(Symbol.ClosureClose);
                Require(Symbol.SemiColon);
                ScopedProgram block = Block();
                block.AddParameters(parameters);
                return new Procedure(ident, parameters, block, null);
            }
            return null;
        }

        private Procedure ReadFunction()
        {
            if (Accept(Symbol.Function))
            {
                Identifier ident = Identifier();
                Require(Symbol.ClosureOpen);
                Parameters parameters = ReadParameters();
                Require(Symbol.ClosureClose);
                Require(Symbol.Colon);
                MiniPascalType type = ReadType();
                if (type == null)
                {
                    throw new SyntaxException(expType, current);
                }
                Require(Symbol.SemiColon);
                ScopedProgram block = Block();
                block.AddParameters(parameters);
                return new Procedure(ident, parameters, block, type);
            }
            return null;
        }

        private DeclarationStatement VariableDeclaration()
        {
            if (Accept(Symbol.Variable))
            {
                List<Identifier> ids = new List<Identifier>();
                do
                {
                    Identifier identifier = Identifier();
                    if (identifier == null)
                    {
                        throw new SyntaxException(expIdentifier, current);
                    }
                    ids.Add(identifier);
                } while (Accept(Symbol.Comma));

                Require(Symbol.Colon);
                MiniPascalType type = ReadType();
                if (type == null)
                {
                    throw new SyntaxException(expType, current);
                }
                return new DeclarationStatement(ids, type);
            }
            return null;
        }

        private Arguments ReadArguments()
        {
            Arguments args = new Arguments();
            do
            {
                IExpression ident = ReadExpression();
                if (ident == null)
                {
                    throw new SyntaxException(expExpression, current);
                }
                args.Add(ident);
            } while (Accept(Symbol.Comma));

            return args;
        }

        private Variable ReadVariable()
        {
            bool isReference = Accept(Symbol.Variable);

            Identifier identifier = Identifier();
            if (identifier == null)
            {
                throw new SyntaxException(expIdentifier, current);
            }

            Require(Symbol.Colon);
            MiniPascalType type = ReadType();
            if (type == null)
            {
                throw new SyntaxException(expType, current);
            }
            return new Variable(identifier, type, isReference);
        }

        private Parameters ReadParameters()
        {
            Parameters parameters = new Parameters();
            do
            {
                Variable variable = ReadVariable();
                parameters.Add(variable);
            } while (Accept(Symbol.Comma));
            return parameters;
        }
    }
}
