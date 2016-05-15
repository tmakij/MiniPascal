using System;

namespace MiniPascal.Lexer.ScannerStates
{
    public sealed class StateStorage
    {
        public bool IsInNestedComment { get { return commentLevel > 0; } }
        private int commentLevel;

        public IScannerState Base { get; } = new Base();
        public IScannerState NestedCommentStart { get; } = new NestedCommentStart();
        public IScannerState Comment { get; } = new Comment();
        public IScannerState CommentEnd { get; } = new CommentEnd();
        public IScannerState Identifier { get; } = new Identifier();
        public IScannerState Colon { get; } = new Colon();
        public IScannerState IntegerLiteral { get; } = new IntegerLiteral();
        public IScannerState StringLiteral { get; } = new StringLiteral();
        public IScannerState SingleLineComment { get; } = new SingleLineComment();
        public IScannerState ForwardSlash { get; } = new ForwardSlash();
        public IScannerState EscapeCharacter { get; } = new EscapeCharacter();
        public IScannerState RealLiteral { get; } = new RealLiteral();
        public IScannerState ExponentSign { get; } = new ExponentSign();
        public IScannerState Exponent { get; } = new Exponent();
        public IScannerState Less { get; } = new Less();
        public IScannerState Greater { get; } = new Greater();
        public IScannerState B { get; } = new B();
        public IScannerState E { get; } = new E();
        public IScannerState I { get; } = new I();
        public IScannerState W { get; } = new W();

        public IScannerState REA { get; }

        public IScannerState F { get; }
        public IScannerState T { get; }
        public IScannerState P { get; }
        public IScannerState S { get; }
        public IScannerState O { get; }
        public IScannerState A { get; }

        public IScannerState If { get; } = new SingleStateEnd(Symbol.If);
        public IScannerState Integer { get; }
        public IScannerState Write { get; }
        public IScannerState Variable { get; }
        public IScannerState End { get; }
        public IScannerState Boolean { get; }
        public IScannerState Do { get; }

        public IScannerState PROcedure { get; }
        public IScannerState Function { get; }
        public IScannerState PROgram { get; }
        public IScannerState Begin { get; }
        public IScannerState While { get; }
        public IScannerState Else { get; }
        public IScannerState Of { get; }

        public IScannerState And { get; }
        public IScannerState Or { get; }
        public IScannerState Not { get; }

        public StateStorage()
        {
            Write = Scanner(Symbol.PrintProcedure, "riteln");
            Variable = Scanner(Symbol.Variable, "var");
            End = Scanner(Symbol.End, "nd");
            Boolean = Scanner(Symbol.BooleanType, "oolean");
            Do = Scanner(Symbol.Do, "do");
            PROcedure = Scanner(Symbol.Procedure, "cedure");
            Function = Scanner(Symbol.Function, "function");
            PROgram = Scanner(Symbol.Program, "gram");
            Begin = Scanner(Symbol.Begin, "egin");
            While = Scanner(Symbol.While, "hile");
            Else = Scanner(Symbol.Else, "lse");
            Of = Scanner(Symbol.Of, "of");
            And = Scanner(Symbol.LogicalAnd, "and");
            Or = Scanner(Symbol.LogicalOr, "or");
            Not = Scanner(Symbol.LogicalNot, "not");
            Integer = Scanner(Symbol.IntegerType, "nteger");

            T = new CharacterSplit(Value("hen", Symbol.Then), Value("rue", Symbol.BooleanLiteral));
            F = new CharacterSplit(Value("alse", Symbol.BooleanLiteral), Value("unction", Symbol.Function));

            IScannerState pro = new Pro();
            P = Scanner(pro, "pro");

            IScannerState realRead = new CharacterSplit(Value("l", Symbol.RealType), Value("d", Symbol.ReadProcedure));
            REA = Scanner(realRead, "rea");

            S = new CharacterSplit(Value("tring", Symbol.StringType), Value("ize", Symbol.Size));
            O = new CharacterSplit(Value("f", Symbol.Of), Value("r", Symbol.LogicalOr));
            A = new CharacterSplit(Value("nd", Symbol.LogicalAnd), Value("ssert", Symbol.Assert), Value("rray", Symbol.Array));
        }

        private static Tuple<char, IScannerState> Value(string Rest, Symbol Result)
        {
            return Tuple.Create(Rest[0], Scanner(Result, Rest));
        }

        private static IScannerState Scanner(IScannerState End, string Moves)
        {
            IScannerState curr = End;
            for (int i = Moves.Length - 1; i > 0; i--)
            {
                curr = new SingleState(Moves[i], curr);
            }
            return curr;
        }

        private static IScannerState Scanner(Symbol End, string Moves)
        {
            return Scanner(new SingleStateEnd(End), Moves);
        }

        public void IncreaseLevel()
        {
            commentLevel++;
        }

        public void DecreaseLevel()
        {
            commentLevel--;
        }
    }
}
