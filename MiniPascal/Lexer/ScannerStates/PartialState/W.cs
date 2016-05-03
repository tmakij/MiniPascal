namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class W : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == 'h')
            {
                Current.Append(Read);
                return States.While;
            }
            if (Read == 'r')
            {
                Current.Append(Read);
                return States.Write;
            }
            return States.Identifier.Read(Current, Read, States);
        }
    }
}
