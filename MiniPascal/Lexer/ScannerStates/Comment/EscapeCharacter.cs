namespace MiniPL.Lexer.ScannerStates
{
    public sealed class EscapeCharacter : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            switch (Read)
            {
                case 'a':
                    Current.Append('\a');
                    return States.StringLiteral;
                case 'n':
                    Current.AppendNewLine();
                    return States.StringLiteral;
                case '"':
                    Current.Append('"');
                    return States.StringLiteral;
                case '\\':
                    Current.Append('\\');
                    return States.StringLiteral;
                default:
                    Current.Append('\\');
                    return States.StringLiteral.Read(Current, Read, States);
            }
        }
    }
}
