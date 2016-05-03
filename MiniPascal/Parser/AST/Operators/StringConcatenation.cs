using System;

namespace MiniPascal.Parser.AST
{
    public sealed class StringConcatenation : IBinaryOperator
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.String; } }

        public void EmitIR(CILEmitter Emitter)
        {
            throw new NotImplementedException();
        }
    }
}
