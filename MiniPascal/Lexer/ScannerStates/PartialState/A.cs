namespace MiniPL.Lexer.ScannerStates
{
    public sealed class A : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == 's')
            {
                Current.Append(Read);
                return States.Assert;
            }
            if (Read == 'n')
            {
                Current.Append(Read);
                return States.And;
            }
            if (Read == 'r')
            {
                Current.Append(Read);
                return States.Array;
            }
            return States.Identifier.Read(Current, Read, States);
        }
    }
}
