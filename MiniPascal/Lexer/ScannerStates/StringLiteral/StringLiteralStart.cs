namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class StringLiteralStart : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '"')
            {
                return States.StringLiteral;
            }
            throw new LexerException("Invalid string literal start");
        }
    }
}
