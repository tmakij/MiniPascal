using MiniPascal.Parser.AST;
using MiniPascal.Lexer;
using System;

namespace MiniPascal.Parser
{
    public sealed partial class Parser
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
            Identifier ident = Identifier();
            if (ident == null)
            {
                throw new SyntaxException("Program must have a name");
            }
            Require(Symbol.SemiColon);
            ScopedProgram block = Block();
            Require(Symbol.Period);
            return new AbstractSyntaxTree(block, ident);
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
                    //Console.WriteLine(Environment.StackTrace);
                    Require(Symbol.End);
                    break;
                }
                if (Accept(Symbol.End))
                {
                    //Console.WriteLine(Environment.StackTrace);
                    break;
                }
            }

            return block;
        }

        private MiniPascalType Type()
        {
            if (Accept(Symbol.IntegerType))
            {
                return MiniPascalType.Integer;
            }
            if (Accept(Symbol.StringType))
            {
                return MiniPascalType.String;
            }
            if (Accept(Symbol.BooleanType))
            {
                return MiniPascalType.Boolean;
            }
            if (Accept(Symbol.RealType))
            {
                return MiniPascalType.Real;
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

        private bool AcceptWithLexeme(Symbol Accepted, out String Lexeme)
        {
            Lexeme = tokens.Current.Lexeme;
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
                throw new SyntaxException(Expected.ToString(), symbol);
            }
        }
    }
}
