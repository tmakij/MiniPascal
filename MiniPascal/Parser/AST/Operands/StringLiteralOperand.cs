﻿namespace MiniPL.Parser.AST
{
    public sealed class StringLiteralOperand : IOperand
    {
        private readonly string literal;

        public StringLiteralOperand(string Literal)
        {
            literal = Literal;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
        }

        public MiniPascalType NodeType(IdentifierTypes Types)
        {
            return MiniPascalType.String;
        }

        public ReturnValue Execute(Variables Global)
        {
            return new ReturnValue(MiniPascalType.String, literal);
        }

        public void EmitIR(CILEmitter Emitter)
        {
            throw new System.NotImplementedException();
        }
    }
}
