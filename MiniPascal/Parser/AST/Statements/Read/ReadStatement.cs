using System;

namespace MiniPascal.Parser.AST
{
    public sealed class ReadStatement : IStatement
    {
        private readonly Identifier variableToReadInto;

        public ReadStatement(Identifier VariableToReadInto)
        {
            variableToReadInto = VariableToReadInto;
        }

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            if (!Used.IsUsed(variableToReadInto))
            {
                throw new UninitializedVariableException(variableToReadInto);
            }
            /*if (!Used.IsMutable(variableToReadInto))
            {
                throw new ImmutableVariableException(variableToReadInto);
            }*/
        }

        public void CheckType(IdentifierTypes Types)
        {
            MiniPascalType varType = Types.GetIdentifierType(variableToReadInto);
            if (!varType.Equals(MiniPascalType.Integer) && !varType.Equals(MiniPascalType.String))
            {
                throw new InvalidTypeException(varType, MiniPascalType.Integer, MiniPascalType.String);
            }
        }

        public void EmitIR(CILEmitter Emitter, IdentifierTypes Types)
        {
            throw new NotImplementedException();
        }
    }
}
