namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class Exponent : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (char.IsNumber(Read))
            {
                Current.Append(Read);
                return this;
            }
            Current.End(Symbol.RealLiteral);
            return States.Base.Read(Current, Read, States);
        }
    }
}
