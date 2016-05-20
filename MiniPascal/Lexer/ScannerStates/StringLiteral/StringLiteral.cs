namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class StringLiteral : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '\\')
            {
                return States.EscapeCharacter;
            }
            Current.Append(Read);
            return this;
        }
    }
}
