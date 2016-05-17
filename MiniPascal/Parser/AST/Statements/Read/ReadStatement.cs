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
            /*
            SimpleType varType = Current.Variable(variableToReadInto).Type;
            if (!varType.Equals(SimpleType.Integer) && !varType.Equals(SimpleType.String))
            {
                throw new InvalidTypeException(varType, SimpleType.Integer, SimpleType.String);
            }
            */
        }

        public void EmitIR(CILEmitter Emitter)
        {
            throw new NotImplementedException();
        }
    }
}
