namespace MiniPL.Lexer.ScannerStates
{
    public sealed class SingleLineComment : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '\n')
            {
                return States.Base;
            }
            return this;
        }
    }
}
