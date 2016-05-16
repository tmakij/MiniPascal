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

        public void CheckIdentifiers(Scope Current)
        {
            if (!Current.IsUsed(variableToReadInto))
            {
                throw new UninitializedVariableException(variableToReadInto);
            }
            /*if (!Current.IsMutable(variableToReadInto))
            {
                throw new ImmutableVariableException(variableToReadInto);
            }*/
        }

        public void CheckType(Scope Current)
        {
            MiniPascalType varType = Current.Variable(variableToReadInto).Type;
            if (!varType.Equals(MiniPascalType.Integer) && !varType.Equals(MiniPascalType.String))
            {
                throw new InvalidTypeException(varType, MiniPascalType.Integer, MiniPascalType.String);
            }
        }

        public void EmitIR(CILEmitter Emitter)
        {
            throw new NotImplementedException();
        }
    }
}
