namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class E : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == 'l')
            {
                Current.Append(Read);
                return States.Else;
            }
            if (Read == 'n')
            {
                Current.Append(Read);
                return States.End;
            }
            return States.Identifier.Read(Current, Read, States);
        }
    }
}
