namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class Base : IScannerState
    {
        IScannerState IScannerState.Read(TokenConstruction Current, char Read, StateStorage States)
        {
            if (char.IsWhiteSpace(Read))
            {
                return this;
            }
            if (char.IsLetter(Read))
            {
                Current.Append(Read);
                switch (Read)
                {
                    case 'a':
                        /*assert, and, array*/
                        return States.A;
                    case 'b':
                        /*begin, boolean */
                        return States.B;
                    case 'd':
                        return States.Do;
                    case 'e':
                        /* else, end */
                        return States.E;
                    case 'f':
                        /* false, function*/
                        return States.F;
                    case 'i':
                        /*integer, if*/
                        return States.I;
                    case 'n':
                        return States.Not;
                    case 'o':
                        return States.Of;
                    case 'p':
                        /* procedure, program */
                        return States.P;
                    case 'r':
                        /* real, read*/
                        return States.REA;
                    case 's':
                        /* size, string*/
                        return States.S;
                    case 't':
                        /* then, true */
                        return States.T;
                    case 'v':
                        return States.Variable;
                    case 'w':
                        /* while, writeln*/
                        return States.W;
                    default:
                        return States.Identifier;
                }
            }
            if (char.IsNumber(Read))
            {
                Current.Append(Read);
                return States.IntegerLiteral;
            }
            switch (Read)
            {
                case '/':
                    return States.ForwardSlash;
                case ':':
                    return States.Colon;
                case '"':
                    return States.StringLiteral;
                case '<':
                    return States.Less;
                case '>':
                    return States.Greater;
                case '%':
                    Current.End(Symbol.Modulo);
                    return this;
                case ',':
                    Current.End(Symbol.Comma);
                    return this;
                case '.':
                    Current.End(Symbol.Period);
                    return this;
                case '&':
                    Current.End(Symbol.LogicalAnd);
                    return this;
                case '!':
                    Current.End(Symbol.LogicalNot);
                    return this;
                case '=':
                    Current.End(Symbol.Equality);
                    return this;
                case '+':
                    Current.End(Symbol.Addition);
                    return this;
                case '*':
                    Current.End(Symbol.Multiplication);
                    return this;
                case ';':
                    Current.End(Symbol.SemiColon);
                    return this;
                case '(':
                    Current.End(Symbol.ClosureOpen);
                    return this;
                case ')':
                    Current.End(Symbol.ClosureClose);
                    return this;
                case '-':
                    Current.End(Symbol.Substraction);
                    return this;
            }
            throw new LexerException("Invalid character " + Read);
        }
    }
}
