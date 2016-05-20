namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class ForwardSlash : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            Current.End(Symbol.Division);
            return States.Base.Read(Current, Read, States);
        }
    }
}
