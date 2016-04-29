namespace MiniPL.Lexer.ScannerStates
{
    public sealed class Colon : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '=')
            {
                Current.End(Symbol.Assigment);
                return States.Base;
            }
            Current.End(Symbol.Colon);
            return States.Base.Read(Current, Read, States);
        }
    }
}
