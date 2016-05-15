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

        public void CheckIdentifiers(UsedIdentifiers Used)
        {
            first.CheckIdentifiers(Used);
            foreach (OperatorPair<IOperand> opr in toAdd)
            {
                opr.Operand.CheckIdentifiers(Used);
            }
        }

        public MiniPascalType NodeType(IdentifierTypes Types)
        {
            Type = first.NodeType(Types);
            foreach (OperatorPair<IOperand> opr in toAdd)
            {
                MiniPascalType nextType = opr.Operand.NodeType(Types);
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
