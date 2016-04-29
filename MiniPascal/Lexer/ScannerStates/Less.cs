namespace MiniPL.Lexer.ScannerStates
{
    public sealed class Less : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '=')
            {
                Current.End(Symbol.LessOrEqualThan);
                return States.Base;
            }
            if (Read == '>')
            {
                Current.End(Symbol.NotEquals);
                return States.Base;
            }
            Current.End(Symbol.LessThan);
            return States.Base.Read(Current, Read, States);
        }
    }
}
