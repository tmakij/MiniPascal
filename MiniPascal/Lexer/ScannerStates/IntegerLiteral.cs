namespace MiniPL.Lexer.ScannerStates
{
    public sealed class IntegerLiteral : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (char.IsNumber(Read))
            {
                Current.Append(Read);
                return this;
            }
            if (Read == '.')
            {
                Current.Append(Read);
                return States.RealLiteral;
            }
            Current.End(Symbol.IntegerLiteral);
            return States.Base.Read(Current, Read, States);
        }
    }
}
