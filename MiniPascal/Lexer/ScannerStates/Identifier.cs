namespace MiniPL.Lexer.ScannerStates
{
    public sealed class Identifier : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (char.IsLetterOrDigit(Read) || Read == '_')
            {
                Current.Append(Read);
                return this;
            }
            Current.End(Symbol.Identifier);
            return States.Base.Read(Current, Read, States);
        }
    }
}
