﻿using MiniPascal.Parser.AST;
using MiniPascal.Lexer;
using System;

namespace MiniPascal.Parser
{
    public sealed partial class Parser
    {
        private readonly TokenStream tokens;
        private Symbol symbol { get { return tokens.Symbol; } }
        private Token current { get { return tokens.Current; } }

        private const string expExpression = "expression (operand-operator-operand or (unary operator)-operand)";
        private const string expOperand = "operand (variable, literal or expression between closures)";
        private const string expType = "int, real, string or bool";
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
            Identifier ident = Identifier();
            if (ident == null)
            {
                throw new SyntaxException("Program must have a name");
            }
            Require(Symbol.SemiColon);
            ScopedProgram block = Block();
            if (block == null)
            {
                throw new SyntaxException("Program must start with a block");
            }
            Require(Symbol.Period);
            return new AbstractSyntaxTree(block, ident);
        }

        private ScopedProgram Block()
        {
            if (!Accept(Symbol.Begin))
            {
                return null;
            }

            ScopedProgram block = new ScopedProgram();

            while (true)
            {
                IStatement stm = Statement();
                if (stm == null)
                {
                    throw new SyntaxException("beginning of a statement", current);
                }
                block.Add(stm);
                if (!Accept(Symbol.SemiColon))
                {
                    Require(Symbol.End);
                    break;
                }
                if (Accept(Symbol.End))
                {
                    break;
                }
            }

            return block;
        }

        private MiniPascalType ReadType()
        {
            if (Accept(Symbol.Array))
            {
                Require(Symbol.IndexOpen);
                IExpression intLiteral = ReadExpression();
                Require(Symbol.IndexClose);
                Require(Symbol.Of);
                SimpleType type = ReadSimpleType();
                if (type == null)
                {
                    throw new SyntaxException(expType, current);
                }
                return new MiniPascalType(type, intLiteral);
            }
            SimpleType simple = ReadSimpleType();
            if (simple != null)
            {
                if (simple.Equals(SimpleType.Integer))
                {
                    return MiniPascalType.Integer;
                }
                else if (simple.Equals(SimpleType.Boolean))
                {
                    return MiniPascalType.Boolean;
                }
                else if (simple.Equals(SimpleType.Real))
                {
                    return MiniPascalType.Real;
                }
                else if (simple.Equals(SimpleType.String))
                {
                    return MiniPascalType.String;
                }
            }
            return null;
        }

        private SimpleType ReadSimpleType()
        {
            if (Accept(Symbol.IntegerType))
            {
                return SimpleType.Integer;
            }
            if (Accept(Symbol.StringType))
            {
                return SimpleType.String;
            }
            if (Accept(Symbol.BooleanType))
            {
                return SimpleType.Boolean;
            }
            if (Accept(Symbol.RealType))
            {
                return SimpleType.Real;
            }
            return null;
        }

        private VariableReference ReadVariableReference(Identifier Name)
        {
            if (Accept(Symbol.IndexOpen))
            {
                IExpression arrayIndex = ReadExpression();
                Require(Symbol.IndexClose);
                return new VariableReference(Name, arrayIndex);
            }
            return new VariableReference(Name, null);
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

        private bool AcceptWithLexeme(Symbol Accepted, out string Lexeme)
        {
            Lexeme = current.Lexeme;
            return Accept(Accepted);
        }

        private bool Matches(Symbol Expected)
        {
            return symbol == Expected;
        }

        private void Require(Symbol Expected)
        {
            if (!Accept(Expected))
            {
                throw new SyntaxException(Expected.ToString(), current);
            }
        }
    }
}
