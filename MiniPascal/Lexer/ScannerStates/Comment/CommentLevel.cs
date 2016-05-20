namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class CommentLevel
    {
        public bool IsInNestedComment { get { return commentLevel > 0; } }
        private int commentLevel;

        public void Increase()
        {
            commentLevel++;
        }

        public void Decrease()
        {
            commentLevel--;
        }
    }
}
