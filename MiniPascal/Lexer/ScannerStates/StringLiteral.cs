namespace MiniPL.Lexer.ScannerStates
{
    public sealed class StringLiteral : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '"')
            {
                Current.End(Symbol.StringLiteral);
                return States.Base;
            }
            if (Read == '\\')
            {
                return States.EscapeCharacter;
            }
            Current.Append(Read);
            return this;
        }
    }
}
