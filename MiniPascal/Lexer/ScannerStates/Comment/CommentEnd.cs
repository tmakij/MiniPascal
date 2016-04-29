namespace MiniPL.Lexer.ScannerStates
{
    public sealed class CommentEnd : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '/')
            {
                States.DecreaseLevel();
                if (!States.IsInNestedComment)
                {
                    return States.Base;
                }
            }
            return States.Comment;
        }
    }
}
