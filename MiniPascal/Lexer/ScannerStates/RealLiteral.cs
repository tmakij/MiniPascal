namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class RealLiteral : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (char.IsNumber(Read))
            {
                Current.Append(Read);
                return this;
            }
            if (Read == 'e')
            {
                Current.Append(Read);
                return States.ExponentSign;
            }
            Current.End(Symbol.RealLiteral);
            return States.Base.Read(Current, Read, States);
        }
    }
}
