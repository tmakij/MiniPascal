namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class I : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == 'n')
            {
                Current.Append(Read);
                return States.Integer;
            }
            if (Read == 'f')
            {
                Current.Append(Read);
                return States.If;
            }
            return States.Identifier.Read(Current, Read, States);
        }
    }
}
