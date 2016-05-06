using System.Collections.Generic;
using System.Text;

namespace MiniPascal.Lexer
{
    public sealed class TokenConstruction
    {
        private readonly StringBuilder curr = new StringBuilder();
        private readonly List<Token> tokens = new List<Token>();

        public TokenStream CreateStream()
        {
            return new TokenStream(tokens.AsReadOnly());
        }

        public void Append(char Character)
        {
            curr.Append(Character);
        }

        public void AppendNewLine()
        {
            curr.AppendLine();
        }

        public void End(Symbol ID)
        {
            string res = TokenText(ID);
            Token t = new Token(res, ID);
            tokens.Add(t);

            string dbg;
            if (ID == Symbol.IntegerLiteral || ID == Symbol.Identifier)
            {
                dbg = ID + ": " + res;
            }
            else
            {
                dbg = ID.ToString();
            }
            System.Console.WriteLine("Read token: " + dbg);

        }

        private string TokenText(Symbol ID)
        {
            switch (ID)
            {
                case Symbol.IntegerLiteral:
                case Symbol.StringLiteral:
                case Symbol.Identifier:
                case Symbol.RealLiteral:
                case Symbol.BooleanLiteral:
                    string text = curr.ToString();
                    curr.Clear();
                    return text;
                default:
                    curr.Clear();
                    return ID.ToString();
            }
        }
    }
}
