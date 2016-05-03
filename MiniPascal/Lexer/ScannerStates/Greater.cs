namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class Greater : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '=')
            {
                Current.End(Symbol.GreaterOrEqualThan);
                return States.Base;
            }
            Current.End(Symbol.GreaterThan);
            return States.Base.Read(Current, Read, States);
        }
    }
}
