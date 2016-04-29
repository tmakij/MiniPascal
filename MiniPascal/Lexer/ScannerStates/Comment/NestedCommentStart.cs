namespace MiniPL.Lexer.ScannerStates
{
    public sealed class NestedCommentStart : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '*')
            {
                States.IncreaseLevel();
                return States.Comment;
            }
            return States.Comment;
        }
    }
}
