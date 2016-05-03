namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class Pro : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == 'c')
            {
                Current.Append(Read);
                return States.PROcedure;
            }
            if (Read == 'g')
            {
                Current.Append(Read);
                return States.PROgram;
            }
            return States.Identifier.Read(Current, Read, States);
        }
    }
}
