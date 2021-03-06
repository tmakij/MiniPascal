﻿namespace MiniPascal.Parser.AST
{
    public sealed class BooleanLiteral : IOperand
    {
        public MiniPascalType Type { get { return MiniPascalType.Boolean; } }

        private readonly bool literal;

        public BooleanLiteral(bool Value)
        {
            literal = Value;
        }

        public void CheckIdentifiers(Scope Current)
        {
        }

        public MiniPascalType NodeType(Scope Current)
        {
            return MiniPascalType.Boolean;
        }

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            if (Reference)
            {
                throw new InvalidByReferenceException();
            }
            int booleanValue = literal ? 1 : 0;
            Emitter.PushInt32(booleanValue);
        }
    }
}
