namespace MiniPL.Lexer.ScannerStates
{
    public sealed class ForwardSlash : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '*')
            {
                States.IncreaseLevel();
                return States.Comment;
            }
            if (Read == '/')
            {
                return States.SingleLineComment;
            }
            Current.End(Symbol.Division);
            return States.Base.Read(Current, Read, States);
        }
    }
}
