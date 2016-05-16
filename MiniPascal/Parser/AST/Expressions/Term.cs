using System.Collections.Generic;

namespace MiniPascal.Parser.AST
{
    public sealed class Term
    {
        public MiniPascalType Type { get; private set; }
        private readonly IOperand first;
        private readonly List<OperatorPair<IOperand>> toAdd = new List<OperatorPair<IOperand>>();

        public Term(IOperand Factor)
        {
            first = Factor;
        }

        public void Add(OperatorPair<IOperand> Factor)
        {
            toAdd.Add(Factor);
        }

        public void CheckIdentifiers(Scope Current)
        {
            first.CheckIdentifiers(Current);
            foreach (OperatorPair<IOperand> opr in toAdd)
            {
                opr.Operand.CheckIdentifiers(Current);
            }
        }

        public MiniPascalType NodeType(Scope Current)
        {
            Type = first.NodeType(Current);
            foreach (OperatorPair<IOperand> opr in toAdd)
            {
                MiniPascalType nextType = opr.Operand.NodeType(Current);
                if (!Type.Equals(nextType))
                {
                    throw new InvalidTypeException(nextType, Type);
                }
                if (!Type.HasOperatorDefined(opr.Operator))
                {
                    throw new UndefinedOperatorException(Type, opr.Operator);
                }
                Type = nextType.BinaryOperation(opr.Operator).ReturnType;
                /*MiniPascalType operationResultType = nextType.BinaryOperation(opr.Operator).ReturnType;
                if (!Type.Equals(operationResultType))
                {
                    throw new InvalidTypeException(operationResultType, Type);
                }*/
            }
            return Type;
        }

        public void EmitIR(CILEmitter Emitter, bool Reference)
        {
            first.EmitIR(Emitter, Reference);
            foreach (OperatorPair<IOperand> factor in toAdd)
            {
                factor.Operand.EmitIR(Emitter, Reference);
                Type.BinaryOperation(factor.Operator).EmitIR(Emitter);
            }
        }
    }
}
