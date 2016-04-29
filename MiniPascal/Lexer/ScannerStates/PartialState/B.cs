namespace MiniPL.Lexer.ScannerStates
{
    public sealed class B : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == 'o')
            {
                Current.Append(Read);
                return States.Boolean;
            }
            if (Read == 'e')
            {
                Current.Append(Read);
                return States.Begin;
            }
            return States.Identifier.Read(Current, Read, States);
        }
    }
}
