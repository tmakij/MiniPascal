﻿namespace MiniPascal.Lexer
{
    public enum Symbol
    {
        EndOfInput,
        Identifier,
        Variable,
        IntegerLiteral,
        Colon,
        Assigment,
        Addition,
        Multiplication,
        ClosureOpen,
        ClosureClose,
        SemiColon,
        IntegerType,
        PrintProcedure,
        StringLiteral,
        ReadProcedure,
        Substraction,
        Equality,
        End,
        Assert,
        StringType,
        BooleanType,
        Do,
        Division,
        LogicalAnd,
        LogicalNot,
        LessThan,

        RealLiteral,
        RealType,
        BooleanLiteral,
        NotEquals,
        GreaterThan,
        LessOrEqualThan,
        GreaterOrEqualThan,
        IndexOpen,
        IndexClose,
        Array,
        Procedure,
        Function,
        Program,
        Begin,
        While,
        If,
        Else,
        Then,
        Of,
        LogicalOr,
        Period,
        Size,
        Comma,
        Return,
        Modulo
    }
}
