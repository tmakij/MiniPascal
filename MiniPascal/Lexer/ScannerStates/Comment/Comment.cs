namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class Comment : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == '*')
            {
                return States.CommentClose;
            }
            if (Read == '{')
            {
                return States.CommentOpen;
            }
            return this;
        }
    }
}
