namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class CommentClose : IScannerState
    {
        private readonly CommentLevel level;

        public CommentClose(CommentLevel Level)
        {
            level = Level;
        }

        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '}')
            {
                level.Decrease();
                if (!level.IsInNestedComment)
                {
                    return States.Base;
                }
            }
            return States.Comment;
        }
    }
}
