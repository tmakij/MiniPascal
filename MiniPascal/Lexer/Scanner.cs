using MiniPascal.Lexer.ScannerStates;

namespace MiniPascal.Lexer
{
    public sealed class Scanner
    {
        private readonly SourceStream source;

        public Scanner(SourceStream Source)
        {
            source = Source;
        }

        public TokenStream GenerateTokens()
        {
            StateStorage scannerStates = new StateStorage();
            TokenConstruction constr = new TokenConstruction();
            IScannerState currentState = scannerStates.Base;
            while (true)
            {
                source.MoveNext();
                if (source.EndOfStream)
                {
                    break;
                }
                char curr = source.Current;
                if (curr == '\n')
                {
                    constr.IncrementLines();
                }
                if (!currentState.Equals(scannerStates.StringLiteral))
                {
                    curr = char.ToLowerInvariant(curr);
                }
                currentState = currentState.Read(constr, curr, scannerStates);
            }
            if (!currentState.Equals(scannerStates.Base))
            {
                throw new LexerException("Unexpected end of input");
            }
            constr.End(Symbol.EndOfInput);
            return constr.CreateStream();
        }
    }
}
