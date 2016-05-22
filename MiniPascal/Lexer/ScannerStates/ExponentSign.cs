namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class ExponentSign : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '+' || Read == '-' || char.IsNumber(Read))
            {
                Current.Append(Read);
                return States.Exponent;
            }
            throw new LexerException("Expected digits or sign, found " + Read, Current.Line);
        }
    }
}
