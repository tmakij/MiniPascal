namespace MiniPL.Lexer.ScannerStates
{
    public sealed class Comment : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '*')
            {
                return States.CommentEnd;
            }
            if (Read == '/')
            {
                return States.NestedCommentStart;
            }
            return this;
        }
    }
}
