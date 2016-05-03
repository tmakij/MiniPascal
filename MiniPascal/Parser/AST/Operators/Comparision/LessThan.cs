using System;

namespace MiniPascal.Parser.AST
{
    public sealed class LessThan<T> : IBinaryOperator where T : IComparable<T>
    {
        public MiniPascalType ReturnType { get { return MiniPascalType.Boolean; } }

        public void EmitIR(CILEmitter Emitter)
        {
            Emitter.CallComparable(MiniPascalType.Integer);
        }
    }
}
