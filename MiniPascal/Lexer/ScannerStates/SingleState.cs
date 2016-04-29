namespace MiniPL.Lexer.ScannerStates
{
    public sealed class SingleState : IScannerState
    {
        private readonly char nextChar;
        private readonly IScannerState nextState;

        public SingleState(char NextChar, IScannerState NextState)
        {
            nextChar = NextChar;
            nextState = NextState;
        }

        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (Read == nextChar)
            {
                Current.Append(Read);
                return nextState;
            }
            return States.Identifier.Read(Current, Read, States);
        }
    }
}
