namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class CommentOpen : IScannerState
    {
        private readonly CommentLevel level;

        public CommentOpen(CommentLevel Level)
        {
            level = Level;
        }

        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '*')
            {
                level.Increase();
                return States.Comment;
            }
            if (level.IsInNestedComment)
            {
                return States.Comment;
            }
            throw new LexerException("Invalid comment start", Current.Line);
        }
    }
}
